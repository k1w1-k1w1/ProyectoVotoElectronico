using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Voto
    {
        public int Id { get; set; }
        public int EleccionId { get; set; }

        public int? CandidatoId { get; set; }

        public int? ListaPoliticaId { get; set; }

        public DateTime FechaHora { get; set; }

    }
}
