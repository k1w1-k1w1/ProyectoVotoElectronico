using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class ListaPolitica
    {
        [Key]public int Idlista { get; set; }
        public string NombreLista { get; set; } 
        public int EleccionId { get; set; }
        public Eleccion Eleccion { get; set; }

    }
}
