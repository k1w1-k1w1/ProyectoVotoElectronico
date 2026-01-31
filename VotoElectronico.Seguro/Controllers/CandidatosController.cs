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
    private async Task CargarEleccionesEnViewBag()
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var elecciones = await client.GetFromJsonAsync<List<Eleccion>>("api/Elecciones");
        if (elecciones != null)
        {
            ViewBag.Elecciones = new SelectList(elecciones, "IdEleccion", "Nombre");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarEleccionesEnViewBag(); 
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CandidatoCreateViewModel model)
    {
        if (model.Foto == null)
        {
            ModelState.Remove("Foto");
        }

        if (ModelState.IsValid)
        {
            string rutaFoto = "/img/default-user.png"; 

            if (model.Foto != null && model.Foto.Length > 0)
            {
                rutaFoto = await GuardarArchivo(model.Foto, "fotos");
            }

            var nuevoCandidato = new
            {
                IdEleccion = model.IdEleccion,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Propuesta = model.Propuesta,
                Cargo = model.Cargo,
                FotoUrl = rutaFoto
            };

            var client = _httpClientFactory.CreateClient("ApiVoto");
            var response = await client.PostAsJsonAsync("api/Candidatos", nuevoCandidato);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Elecciones");
            }
            else
            {
                // LEER EL ERROR REAL
                var cuerpoError = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "La API dice: " + cuerpoError);
            }

            // Si llegamos aquí, la API dio error (probablemente 400 o 500)
            ModelState.AddModelError("", "La API rechazó la solicitud. Verifica los datos.");
        }

        await CargarEleccionesEnViewBag();
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