namespace ProyectoVotoElectronico
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public string Rol { get; set; } 

    }
}
