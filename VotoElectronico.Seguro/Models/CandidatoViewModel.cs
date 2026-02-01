using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VotoElectronico.Seguro.Models
{
    public class CandidatoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una lista política")]
        [Display(Name = "Lista Política")]
        public int ListaPoliticaId { get; set; }

        [Display(Name = "Foto del Candidato")]
        public IFormFile? Foto { get; set; }
    }
}