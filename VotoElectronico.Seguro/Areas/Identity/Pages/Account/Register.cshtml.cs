// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using VotoElectronico.Seguro.Models;
using VotoElectronico.Seguro.Models.Dto;

namespace VotoElectronico.Seguro.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            public string Cedula { get; set; }

            [Required]
            public string Nombre { get; set; }

            [Required]
            public string Apellido { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Telefono { get; set; }

            [Required, Range(16, 120)]
            public int Edad { get; set; }

            [Required]
            public string Ciudad { get; set; }

            [Required, StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
                return Page();

            // 1️⃣ Crear usuario Identity
            var user = CreateUser();

            user.Cedula = Input.Cedula;
            user.Nombre = Input.Nombre;
            user.Apellido = Input.Apellido;
            user.Edad = Input.Edad;
            user.Ciudad = Input.Ciudad;
            user.PhoneNumber = Input.Telefono;

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded) 
            {
                await _userManager.AddToRoleAsync(user, "Votante");

            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return Page();
            }

            // 2️⃣ Registrar votante en la API
            var client = _httpClientFactory.CreateClient("ApiVoto");

            var votanteDto = new VotanteCreateDto
            {
                IdentityUserId = user.Id,
                Cedula = Input.Cedula,
                Nombre = Input.Nombre,
                Apellido = Input.Apellido,
                Edad = Input.Edad,
                Ciudad = Input.Ciudad,
                Email = Input.Email
            };

            var response = await client.PostAsJsonAsync("api/Usuarios", votanteDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetalle = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, $"Error de la API ({response.StatusCode}): {errorDetalle}");

                await _userManager.DeleteAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirma tu voto electrónico",
                $"Por favor confirma tu cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>haciendo clic aquí</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                TempData["StatusMessage"] = "Registro exitoso. Por favor, revisa tu correo para confirmar tu cuenta antes de iniciar sesión.";
                return RedirectToPage("Login");
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

        private ApplicationUser CreateUser()
        {
            return Activator.CreateInstance<ApplicationUser>();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("Email no soportado.");

            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
