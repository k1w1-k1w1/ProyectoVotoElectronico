using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class HistorialVoto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EleccionId { get; set; }
        public DateTime FechaParticipacion { get; set; }
        public bool HaVotado { get; set; }
    }
}
