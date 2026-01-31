using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotoElectronico.Seguro.Models;
using System.Net.Http.Json; 

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
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var todas = await client.GetFromJsonAsync<List<Eleccion>>("api/Elecciones");
        var abiertas = todas?.Where(e => e.Estado == "ABIERTA").ToList() ?? new List<Eleccion>();

        return View("Index", abiertas);
    }

    public async Task<IActionResult> Papeleta(int idEleccion)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var email = User.FindFirstValue(ClaimTypes.Email);

        // Buscar al usuario en la API
        var userResponse = await client.GetAsync($"api/Usuarios/ByEmail/{email}");
        if (!userResponse.IsSuccessStatusCode) return View("ErrorUsuarioNoRegistrado");

        var usuarioApi = await userResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        int idUsuario = usuarioApi.GetProperty("idUsuario").GetInt32();

        // Verificar si ya voto
        var yaVotoResponse = await client.GetAsync($"api/Votos/YaVoto/{idUsuario}/{idEleccion}");
        if (yaVotoResponse.IsSuccessStatusCode)
        {
            var yaVoto = await yaVotoResponse.Content.ReadFromJsonAsync<bool>();
            if (yaVoto) return View("YaVotaste");
        }

        //listas y candidatos de la elección
        var listas = await client.GetFromJsonAsync<List<dynamic>>($"api/ListasPoliticas/eleccion/{idEleccion}");

        ViewBag.IdUsuario = idUsuario;
        ViewBag.IdEleccion = idEleccion;

        return View("Papeleta", listas);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarVoto(int idLista, int idUsuario, int idCandidato, int idEleccion)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var request = new
        {
            IdUsuario = idUsuario,
            IdEleccion = idEleccion,
            IdCandidato = idCandidato,
            IdLista = idLista
        };

        var response = await client.PostAsJsonAsync("api/Votos/EmitirVoto", request);

        if (response.IsSuccessStatusCode)
        {
            var resultado = await response.Content.ReadFromJsonAsync<dynamic>();
            TempData["VotoHash"] = resultado.GetProperty("comprobanteHash").GetString();
            return RedirectToAction("Confirmacion");
        }

        var errorReal = await response.Content.ReadAsStringAsync();
        TempData["Error"] = $"Error al registrar: {errorReal}";

        return RedirectToAction("Papeleta", new { idEleccion = idEleccion });
    }

    public IActionResult Confirmacion()
    {
        ViewBag.Hash = TempData["VotoHash"];
        return View();
    }
}