using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class RegistroVotacion
    {
        public Guid IdRegistro { get; set; }
        public DateTime FechaHora { get; set; }

        public Guid IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public Guid IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
