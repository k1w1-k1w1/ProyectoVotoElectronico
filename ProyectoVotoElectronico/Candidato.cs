using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Candidato
    {
        public Guid IdCandidato { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Propuesta { get; set; }
        public string FotoUrl { get; set; }

        // Relación
        public Guid IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
