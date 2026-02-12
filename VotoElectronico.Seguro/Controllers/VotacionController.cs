using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json; 
using System.Security.Claims;
using System.Text.Json;
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
        try
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");
            var response = await client.GetAsync("api/Elecciones");

            if ((int)response.StatusCode == 429)
            {
                TempData["MensajeError"] = "Servidor ocupado. Por favor, intenta de nuevo en un minuto.";
                return View("Index", new List<Eleccion>());
            }

            response.EnsureSuccessStatusCode();
            var todas = await response.Content.ReadFromJsonAsync<List<Eleccion>>();
            var abiertas = todas?.Where(e => e.Estado == "ABIERTA").ToList() ?? new List<Eleccion>();

            return View("Index", abiertas);
        }
        catch
        {
            return View("Index", new List<Eleccion>());
        }
    }

    public async Task<IActionResult> Papeleta(int idEleccion)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");
            var email = User.FindFirstValue(ClaimTypes.Email);

            // 1. Obtener Usuario
            var userResponse = await client.GetAsync($"api/Usuarios/ByEmail/{email}");
            if ((int)userResponse.StatusCode == 429) goto Error429;
            if (!userResponse.IsSuccessStatusCode) return View("ErrorUsuarioNoRegistrado");

            var usuarioApi = await userResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            int idUsuario = usuarioApi.GetProperty("IdUsuario").GetInt32();

            // 2. Verificar si ya votó
            var yaVotoResponse = await client.GetAsync($"api/Votos/YaVoto/{idUsuario}/{idEleccion}");
            if ((int)yaVotoResponse.StatusCode == 429) goto Error429;

            if (yaVotoResponse.IsSuccessStatusCode)
            {
                var yaVoto = await yaVotoResponse.Content.ReadFromJsonAsync<bool>();
                if (yaVoto) return View("YaVotaste");
            }

            // 3. Obtener Listas (Agregamos un pequeño delay preventivo)
            await Task.Delay(100);
            var listasResponse = await client.GetAsync($"api/ListasPoliticas/eleccion/{idEleccion}");
            if ((int)listasResponse.StatusCode == 429) goto Error429;

            var listas = await listasResponse.Content.ReadFromJsonAsync<List<dynamic>>();

            ViewBag.IdUsuario = idUsuario;
            ViewBag.IdEleccion = idEleccion;

            return View("Papeleta", listas);
        }
        catch (Exception)
        {
            return View("Error");
        }

    Error429:
        TempData["MensajeError"] = "Exceso de peticiones. Espera un momento.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarVoto(int idUsuario, int idEleccion, int idLista, int idCandidato)
    {
        if (idCandidato == 0)
        {
            TempData["MensajeError"] = "Error: La lista seleccionada no tiene un candidato válido.";
            return RedirectToAction("Papeleta", new { idEleccion = idEleccion });
        }
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var nuevoVoto = new
        {
            IdUsuario = idUsuario,
            IdEleccion = idEleccion,
            IdCandidato = idCandidato, 
            IdLista = idLista
        };

        var response = await client.PostAsJsonAsync("api/Votos/EmitirVoto", nuevoVoto);

        if (response.IsSuccessStatusCode)
        {
            var resultado = await response.Content.ReadFromJsonAsync<JsonElement>();
            TempData["VotoHash"] = resultado.GetProperty("comprobante").GetString();

            return RedirectToAction("Confirmacion");
        }
        else
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            TempData["MensajeError"] = "La API dice: " + errorMsg;
            return RedirectToAction("Papeleta", new { idEleccion = idEleccion });
        }
    }

    public IActionResult Confirmacion()
    {
        return View();
    }

    public IActionResult Resultados(int idEleccion)
    {
        ViewBag.IdEleccion = idEleccion;
        return View();
    }
}