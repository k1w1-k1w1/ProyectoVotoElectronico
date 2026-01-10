using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Voto
    {
        [Key]public int IdVoto { get; set; }
        public DateTime FechaHora { get; set; }
        public string HashVoto { get; set; } // voto anonimizado

        // Relaciones
        public int IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }

        public int IdCandidato { get; set; }
        public Candidato Candidato { get; set; }

    }
}
