using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voto;
using VotoElectronico.Api.Models;

namespace voto
{
    public class APIContext : IdentityDbContext
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProyectoVotoElectronico.Candidato>(entity =>
            {
                entity.ToTable("Candidatos");

                entity.Property(c => c.IdEleccion).HasColumnName("IdEleccion");
                entity.Property(c => c.IdLista).HasColumnName("IdLista");


                entity.HasOne(c => c.Eleccion)
                      .WithMany()
                      .HasForeignKey(c => c.IdEleccion)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.ListaPolitica)
                      .WithMany()
                      .HasForeignKey(c => c.IdLista)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }

}
