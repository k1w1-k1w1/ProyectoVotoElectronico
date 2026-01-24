using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class RegistroVotacion
    {
        [Key]
        public int IdRegistro { get; set; }

        public int IdUsuario { get; set; }

        public int IdEleccion { get; set; }

        public DateTime FechaHora { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }
    }
}
