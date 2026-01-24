using Microsoft.AspNetCore.Identity;

namespace VotoElectronico.Seguro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Ciudad { get; set; } = string.Empty;
    }
}
