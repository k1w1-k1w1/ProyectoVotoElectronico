using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Eleccion
    {
        [Key]public int IdEleccion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Tipo { get; set; } // Nominal / Plancha
        public string Estado { get; set; } // CREADA, ABIERTA, CERRADA

        public List<Candidato> Candidatos { get; set; }
        public List<Voto> Votos { get; set; }
    }
}
