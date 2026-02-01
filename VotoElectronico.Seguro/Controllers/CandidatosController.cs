using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using VotoElectronico.Seguro.Models;
using System.Net.Http.Json;
using ProyectoVotoElectronico;

[Authorize(Roles = "Admin")]
public class CandidatosController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IHttpClientFactory _httpClientFactory;

    public CandidatosController(IWebHostEnvironment hostEnvironment, IHttpClientFactory httpClientFactory)
    {
        _hostEnvironment = hostEnvironment;
        _httpClientFactory = httpClientFactory;
    }

    private async Task CargarEleccionesEnViewBag()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var opciones = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        var elecciones = await client.GetFromJsonAsync<List<ProyectoVotoElectronico.Eleccion>>("api/Elecciones", opciones);
        ViewBag.Elecciones = new SelectList(elecciones ?? new List<ProyectoVotoElectronico.Eleccion>(), "IdEleccion", "Nombre");
    }

    private async Task CargarListasEnViewBag()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var opciones = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        var listas = await client.GetFromJsonAsync<List<ProyectoVotoElectronico.ListaPolitica>>("api/ListasPoliticas", opciones);
        ViewBag.ListasPoliticas = new SelectList(listas ?? new List<ProyectoVotoElectronico.ListaPolitica>(), "Idlista", "NombreLista");
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarEleccionesEnViewBag();
        await CargarListasEnViewBag(); 
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CandidatoCreateViewModel model)
    {
        if (model.Foto == null) ModelState.Remove("Foto");

        if (ModelState.IsValid)
        {
            string rutaFoto = "/img/default-user.png";

            if (model.Foto != null && model.Foto.Length > 0)
            {   
                rutaFoto = await GuardarArchivo(model.Foto, "candidatos");
            }

            var nuevoCandidato = new
            {
                IdEleccion = model.IdEleccion,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Propuesta = model.Propuesta,
                Cargo = model.Cargo,
                FotoUrl = rutaFoto,
                IdLista = model.ListaPoliticaId
                
            };

            var client = _httpClientFactory.CreateClient("ApiVoto");
            var response = await client.PostAsJsonAsync("api/Candidatos", nuevoCandidato);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var cuerpoError = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "La API dice: " + cuerpoError);
            }
        }

        await CargarEleccionesEnViewBag();
        await CargarListasEnViewBag();
        return View(model);
    }

    private async Task<string> GuardarArchivo(IFormFile archivo, string subcarpeta)
    {
        if (archivo == null) return null;
        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", subcarpeta);
        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await archivo.CopyToAsync(fileStream);
        }
        return $"/uploads/{subcarpeta}/{uniqueFileName}";
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var response = await client.DeleteAsync($"api/Candidatos/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["Mensaje"] = "Candidato eliminado correctamente";
        }
        else
        {
            TempData["Error"] = "No se pudo eliminar el candidato";
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");

        var opciones = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        var candidatos = await client.GetFromJsonAsync<List<ProyectoVotoElectronico.Candidato>>("api/Candidatos", opciones);

        return View(candidatos ?? new List<ProyectoVotoElectronico.Candidato>());
    }
}