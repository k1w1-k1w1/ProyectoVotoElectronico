using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;

[Authorize] // Solo usuarios logueados pueden entrar
public class VotacionController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public VotacionController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        // 1. Obtener el email del usuario logueado en Identity
        var email = User.FindFirstValue(ClaimTypes.Email);

        // 2. Buscar al usuario en la API para obtener su IdUsuario real
        var userResponse = await client.GetAsync($"api/Usuarios/ByEmail/{email}");
        if (!userResponse.IsSuccessStatusCode)
        {
            return View("ErrorUsuarioNoRegistrado"); // Si el email no está en la DB de votos
        }

        var userJson = await userResponse.Content.ReadAsStringAsync();
        var usuarioApi = JsonConvert.DeserializeObject<dynamic>(userJson);
        ViewBag.IdUsuario = usuarioApi.idUsuario; // Guardamos el ID para el voto

        // 3. Obtener las listas políticas para mostrar en la papeleta
        var listasResponse = await client.GetAsync("api/ListasPoliticas");
        var listasJson = await listasResponse.Content.ReadAsStringAsync();
        var listas = JsonConvert.DeserializeObject<List<dynamic>>(listasJson);

        return View(listas);
    }
}