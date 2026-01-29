using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotoElectronico.Seguro.Models;
using System.Net.Http.Json;

namespace VotoElectronico.Seguro.Controllers;

[Authorize(Roles = "Admin")]
public class EleccionesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EleccionesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // LISTAR ELECCIONES
    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");
            var elecciones = await client.GetFromJsonAsync<List<Eleccion>>("api/Elecciones");
            return View(elecciones);
        }
        catch (Exception)
        {
            return View(new List<Eleccion>());
        }
    }

    // VISTA PARA CREAR
    [HttpGet]
    public IActionResult Crear() => View();

    // PROCESAR CREACIÓN
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Eleccion eleccion)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");
            var response = await client.PostAsJsonAsync("api/Elecciones", eleccion);

            if (response.IsSuccessStatusCode)
            {
                // Cambiado para que vuelvas a la lista y no al Home
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Error al guardar en la API.");
        }
        return View(eleccion);
    }

    // CIERRE MANUAL
    [HttpPost]
    public async Task<IActionResult> Cerrar(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var response = await client.PostAsync($"api/Elecciones/Cerrar/{id}", null);

        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

        return BadRequest("No se pudo cerrar la elección.");
    }

    // ELIMINACIÓN
    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var response = await client.DeleteAsync($"api/Elecciones/{id}");

        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

        TempData["Error"] = "No se puede eliminar la elección. Es posible que ya tenga votos registrados.";
        return RedirectToAction(nameof(Index));
    }
}