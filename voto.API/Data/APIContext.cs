using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using voto;


namespace voto
{
    public class APIContext : DbContext
    {
        public APIContext (DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<ProyectoVotoElectronico.Rol> Roles { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.Usuario> Usuarios { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.Candidato> Candidatos { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.Voto> Votos { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.Eleccion> Elecciones { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.Resultado> Resultados { get; set; } = default!;
        public DbSet<ProyectoVotoElectronico.ListaPolitica> ListasPoliticas { get; set; } = default!;
        public DbSet<RegistroVotacion> RegistroVotaciones { get; set; } = default!;
    }
}
