using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVotoElectronico
{
    public class Eleccion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa { get; set; }

        public TipoEleccion Tipo { get; set; }

        public ICollection<ListaPolitica> Listas { get; set; }
        public ICollection<Candidato> Candidatos { get; set; }

        public enum TipoEleccion
        {
            Nominal,
            Plancha
        }
    }
}
