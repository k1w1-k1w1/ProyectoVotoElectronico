namespace VotoElectronico.Seguro.Models
{
    public class Eleccion
    {
        public int IdEleccion { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Tipo { get; set; }
    }
}