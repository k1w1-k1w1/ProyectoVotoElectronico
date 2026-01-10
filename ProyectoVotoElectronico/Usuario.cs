using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVotoElectronico
{
    public class Usuario
    {
        [Key]public int IdUsuario { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Codigo2FA { get; set; }
        public DateTime? CodigoExpira { get; set; }


        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; }

    }
}
