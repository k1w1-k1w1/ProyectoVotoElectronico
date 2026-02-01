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
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var todas = await client.GetFromJsonAsync<List<Eleccion>>("api/Elecciones");
        var abiertas = todas?.Where(e => e.Estado == "ABIERTA").ToList() ?? new List<Eleccion>();

        return View("Index", abiertas);
    }

    public async Task<IActionResult> Papeleta(int idEleccion)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var email = User.FindFirstValue(ClaimTypes.Email);

        var userResponse = await client.GetAsync($"api/Usuarios/ByEmail/{email}");
        if (!userResponse.IsSuccessStatusCode) return View("ErrorUsuarioNoRegistrado");

        var usuarioApi = await userResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        int idUsuario = usuarioApi.GetProperty("IdUsuario").GetInt32();

        var yaVotoResponse = await client.GetAsync($"api/Votos/YaVoto/{idUsuario}/{idEleccion}");
        if (yaVotoResponse.IsSuccessStatusCode)
        {
            var yaVoto = await yaVotoResponse.Content.ReadFromJsonAsync<bool>();
            if (yaVoto) return View("YaVotaste");
        }

        var listas = await client.GetFromJsonAsync<List<dynamic>>($"api/ListasPoliticas/eleccion/{idEleccion}");

        ViewBag.IdUsuario = idUsuario;
        ViewBag.IdEleccion = idEleccion;

        return View("Papeleta", listas);
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
}