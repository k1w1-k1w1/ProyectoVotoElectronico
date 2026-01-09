using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Resultado
    {
        public Guid IdResultado { get; set; }
        public int TotalVotos { get; set; }
        public string MetodoAsignacion { get; set; } // D'Hondt, Webster
        public DateTime FechaCalculo { get; set; }

        public Guid IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
