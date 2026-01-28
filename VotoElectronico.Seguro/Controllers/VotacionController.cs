using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using VotoElectronico.Seguro.Models;

[Authorize]
public class VotacionController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<ApplicationUser> _userManager; 

    public VotacionController(IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userIdentity = await _userManager.GetUserAsync(User);
        if (userIdentity == null || !userIdentity.EmailConfirmed) 
        {
            return RedirectToPage("/Account/Manage/Email", new { area = "Identity" });
        }

        var client = _httpClientFactory.CreateClient("ApiVoto");
        var email = User.FindFirstValue(ClaimTypes.Email);

        // 1. Buscar al usuario en la API
        var userResponse = await client.GetAsync($"api/Usuarios/ByEmail/{email}");
        if (!userResponse.IsSuccessStatusCode)
        {
            return View("ErrorUsuarioNoRegistrado");
        }

        var usuarioApi = await userResponse.Content.ReadFromJsonAsync<dynamic>();
        int idUsuario = usuarioApi.GetProperty("idUsuario").GetInt32();
        ViewBag.IdUsuario = idUsuario;

        // 2. Verificar si ya votó (usando la nueva forma)
        var yaVotoResponse = await client.GetAsync($"api/Votos/YaVoto/{idUsuario}");
        if (yaVotoResponse.IsSuccessStatusCode)
        {
            var yaVoto = await yaVotoResponse.Content.ReadFromJsonAsync<bool>();
            if (yaVoto)
            {
                return View("YaVotaste"); // Vista para usuarios que ya cumplieron su voto
            }
        }

        // 3. Obtener las listas políticas
        var listas = await client.GetFromJsonAsync<List<dynamic>>("api/ListasPoliticas/eleccion/1");

        return View(listas);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarVoto(int idLista, int idUsuario, int idCandidato)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        // Este objeto debe coincidir con tu clase 'VotoRequest' de la API
        var request = new
        {
            IdUsuario = idUsuario,
            IdEleccion = 1, // La que creamos hoy
            IdCandidato = idCandidato,
            IdLista = idLista
        };

        // ¡IMPORTANTE!: Agregamos "/EmitirVoto" a la URL
        var response = await client.PostAsJsonAsync("api/Votos/EmitirVoto", request);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Confirmacion");
        }

        // Capturamos el error para verlo en la franja roja
        var errorReal = await response.Content.ReadAsStringAsync();
        TempData["Error"] = $"Error: {errorReal}";
        return RedirectToAction("Index");
    }

    public IActionResult Confirmacion()
    {
        return View(); 
    }
}

