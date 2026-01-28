using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using VotoElectronico.Seguro.Models;
using System.Net.Http.Json;
using System.Net.Http.Json;

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

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var elecciones = await client.GetFromJsonAsync<List<Eleccion>>("api/Elecciones");

        if (elecciones != null)
        {
            ViewBag.Elecciones = new SelectList(elecciones, "IdEleccion", "Nombre");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CandidatoCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Solo guardamos la foto
            string rutaFoto = await GuardarArchivo(model.Foto, "fotos");

            var nuevoCandidato = new
            {
                IdEleccion = model.IdEleccion,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Propuesta = model.Propuesta,
                FotoUrl = rutaFoto 
            };

            var client = _httpClientFactory.CreateClient("ApiVoto");
            var response = await client.PostAsJsonAsync("api/Candidatos", nuevoCandidato);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
        }
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
}