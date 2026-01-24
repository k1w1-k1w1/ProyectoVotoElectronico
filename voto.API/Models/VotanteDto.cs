namespace VotoElectronico.Api.Models
{
    public class Votante
    {
        public int Id { get; set; }  // PK de la API

        public string IdentityUserId { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

