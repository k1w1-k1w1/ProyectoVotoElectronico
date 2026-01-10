using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Candidato
    {
        [Key]public int IdCandidato { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Propuesta { get; set; }
        public string FotoUrl { get; set; }

        // Relación
        public int IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
