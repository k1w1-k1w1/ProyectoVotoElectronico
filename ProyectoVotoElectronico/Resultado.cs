using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Resultado
    {
        [Key]public int IdResultado { get; set; }
        public int TotalVotos { get; set; }
        public string MetodoAsignacion { get; set; } 
        public DateTime FechaCalculo { get; set; }

        public int IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
