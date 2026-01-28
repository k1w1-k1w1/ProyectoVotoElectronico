using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VotoElectronico.Seguro.Models
{
    public class CandidatoCreateViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar una elección.")]
        public int IdEleccion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } 

        [Required(ErrorMessage = "La propuesta es obligatoria.")]
        public string Propuesta { get; set; } 

        [Required(ErrorMessage = "Debe subir una foto del candidato.")]
        public IFormFile Foto { get; set; }
    }
}