using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Candidato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; } 
        public string FotoUrl { get; set; }

        public int ListaPoliticaId { get; set; }
        public ListaPolitica ListaPolitica { get; set; }
    }
}
