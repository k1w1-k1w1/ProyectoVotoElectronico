using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVotoElectronico
{
    public class Voto
    {
        [Key]
        public int IdVoto { get; set; }

        public DateTime FechaHora { get; set; }

        public string HashVoto { get; set; }

        // Relación con Elección
        public int IdEleccion { get; set; }
        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }

        // Relación con Lista 
        public int? IdLista { get; set; }
        [ForeignKey("IdLista")]
        public virtual ListaPolitica? Lista { get; set; }

        // Relación con Candidato 
        public int? IdCandidato { get; set; }
        [ForeignKey("IdCandidato")]
        public virtual Candidato? Candidato { get; set; }
    }
}
