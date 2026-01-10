using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Rol
    {
        [Key]public int IdRol { get; set; }
        public string Nombre { get; set; } 
        public string Descripcion { get; set; }
    }
}
