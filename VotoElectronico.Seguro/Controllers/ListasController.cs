using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using VotoElectronico.Seguro.Models;
using Microsoft.AspNetCore.Hosting;
using ProyectoVotoElectronico;

public class ListasController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IHttpClientFactory _httpClientFactory;

    public ListasController(IWebHostEnvironment hostEnvironment, IHttpClientFactory httpClientFactory)
    {
        _hostEnvironment = hostEnvironment;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Crear(int? idEleccion)
    {
        await CargarEleccionesEnViewBag(idEleccion);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ListaPoliticaViewModel model)
    {
        ModelState.Remove("FotoLogo");

        if (ModelState.IsValid)
        {
            try
            {
                string rutaLogo = "/img/default-logo.png";

                if (model.FotoLogo != null && model.FotoLogo.Length > 0)
                {
                    rutaLogo = await GuardarArchivo(model.FotoLogo, "logos");
                }

                var nuevaLista = new
                {
                    NombreLista = model.NombreLista,
                    Descripcion = model.Descripcion ?? "",
                    UrlLogo = rutaLogo,
                    EleccionId = model.EleccionId
                };

                var client = _httpClientFactory.CreateClient("ApiVoto");
                var response = await client.PostAsJsonAsync("api/ListasPoliticas", nuevaLista);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", "La API dice: " + errorMsg);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
        }

        await CargarEleccionesEnViewBag(model.EleccionId);
        return View(model);
    }

    // GET: Listas/Index
    // GET: Listas/Index
    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");

            var opciones = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Cambio: Usar GetAsync para inspeccionar el Status Code antes de deserializar
            var response = await client.GetAsync("api/ListasPoliticas");

            if ((int)response.StatusCode == 429)
            {
                TempData["Error"] = "Servidor saturado. Por favor, espere un momento para listar las listas políticas.";
                return View(new List<ProyectoVotoElectronico.ListaPolitica>());
            }

            response.EnsureSuccessStatusCode();
            var listas = await response.Content.ReadFromJsonAsync<List<ProyectoVotoElectronico.ListaPolitica>>(opciones);

            return View(listas ?? new List<ProyectoVotoElectronico.ListaPolitica>());
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error de conexión con el servicio de listas.";
            return View(new List<ProyectoVotoElectronico.ListaPolitica>());
        }
    }

    private async Task CargarEleccionesEnViewBag(int? selectedId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiVoto");

            var opciones = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            var response = await client.GetAsync("api/Elecciones");

            if (response.IsSuccessStatusCode)
            {
                var elecciones = await response.Content.ReadFromJsonAsync<List<ProyectoVotoElectronico.Eleccion>>(opciones);
                var filtradas = elecciones?.Where(e => e.Tipo?.ToUpper() == "PLANCHA").ToList() ?? new List<ProyectoVotoElectronico.Eleccion>();
                ViewBag.Elecciones = new SelectList(filtradas, "IdEleccion", "Nombre", selectedId);
            }
            else
            {
                ViewBag.Elecciones = new SelectList(new List<ProyectoVotoElectronico.Eleccion>(), "IdEleccion", "Nombre");
            }
        }
        catch
        {
            ViewBag.Elecciones = new SelectList(new List<ProyectoVotoElectronico.Eleccion>(), "IdEleccion", "Nombre");
        }
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

   
    

    // GET: Listas/Eliminar/5
    public async Task<IActionResult> Eliminar(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiVoto");
        var response = await client.DeleteAsync($"api/ListasPoliticas/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "No se pudo eliminar la lista. Verifique que no tenga candidatos asociados.";
        return RedirectToAction(nameof(Index));
    }

}