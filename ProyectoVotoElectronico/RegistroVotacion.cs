using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class RegistroVotacion
    {
        [Key]public int IdRegistro { get; set; }
        public DateTime FechaHora { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public int IdEleccion { get; set; }
        public Eleccion Eleccion { get; set; }
    }
}
