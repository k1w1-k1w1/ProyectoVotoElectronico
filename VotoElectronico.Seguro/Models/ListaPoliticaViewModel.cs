using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VotoElectronico.Seguro.Models
{
    public class ListaPoliticaViewModel
    {
        [Required(ErrorMessage = "El nombre de la lista es obligatorio.")]
        [Display(Name = "Nombre de la Lista o Partido")]
        public string NombreLista { get; set; }

        [Display(Name = "Descripción / Lema")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una elección.")]
        [Display(Name = "Elección")]
        public int EleccionId { get; set; }

        [Display(Name = "Logo de la Lista")]
        public IFormFile? FotoLogo { get; set; } 
    }
}