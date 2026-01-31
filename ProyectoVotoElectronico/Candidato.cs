using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVotoElectronico
{
    public class Candidato
    {
        [Key]
        public int IdCandidato { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Propuesta { get; set; }
        public string Cargo { get; set; }
        public string FotoUrl { get; set; }

        // Relacion con la Eleccion
        public int IdEleccion { get; set; }
        public virtual Eleccion Eleccion { get; set; }

        // Relacion con la Lista 
        public int? IdLista { get; set; } 

        [ForeignKey("IdLista")] 
        public virtual ListaPolitica ListaPolitica { get; set; }
    }
}