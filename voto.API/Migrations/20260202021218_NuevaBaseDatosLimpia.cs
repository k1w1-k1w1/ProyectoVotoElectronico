using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class NuevaBaseDatosLimpia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elecciones",
                columns: table => new
                {
                    IdEleccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elecciones", x => x.IdEleccion);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "ListasPoliticas",
                columns: table => new
                {
                    Idlista = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreLista = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    UrlLogo = table.Column<string>(type: "text", nullable: true),
                    EleccionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasPoliticas", x => x.Idlista);
                    table.ForeignKey(
                        name: "FK_ListasPoliticas_Elecciones_EleccionId",
                        column: x => x.EleccionId,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalVotos = table.Column<int>(type: "integer", nullable: false),
                    MetodoAsignacion = table.Column<string>(type: "text", nullable: true),
                    FechaCalculo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdEleccion = table.Column<int>(type: "integer", nullable: false),
                    EleccionIdEleccion = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.IdResultado);
                    table.ForeignKey(
                        name: "FK_Resultados_Elecciones_EleccionIdEleccion",
                        column: x => x.EleccionIdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityUserId = table.Column<string>(type: "text", nullable: true),
                    Cedula = table.Column<string>(type: "text", nullable: true),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Apellido = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Edad = table.Column<int>(type: "integer", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: true),
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    IdCandidato = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Apellido = table.Column<string>(type: "text", nullable: true),
                    Propuesta = table.Column<string>(type: "text", nullable: true),
                    Cargo = table.Column<string>(type: "text", nullable: true),
                    FotoUrl = table.Column<string>(type: "text", nullable: true),
                    IdEleccion = table.Column<int>(type: "integer", nullable: false),
                    IdLista = table.Column<int>(type: "integer", nullable: true),
                    EleccionIdEleccion = table.Column<int>(type: "integer", nullable: true),
                    ListaPoliticaIdlista = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.IdCandidato);
                    table.ForeignKey(
                        name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                        column: x => x.EleccionIdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                    table.ForeignKey(
                        name: "FK_Candidatos_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatos_ListasPoliticas_IdLista",
                        column: x => x.IdLista,
                        principalTable: "ListasPoliticas",
                        principalColumn: "Idlista",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Candidatos_ListasPoliticas_ListaPoliticaIdlista",
                        column: x => x.ListaPoliticaIdlista,
                        principalTable: "ListasPoliticas",
                        principalColumn: "Idlista");
                });

            migrationBuilder.CreateTable(
                name: "RegistroVotaciones",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdEleccion = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroVotaciones", x => x.IdRegistro);
                    table.ForeignKey(
                        name: "FK_RegistroVotaciones_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroVotaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votos",
                columns: table => new
                {
                    IdVoto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaHora = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    HashVoto = table.Column<string>(type: "text", nullable: true),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdEleccion = table.Column<int>(type: "integer", nullable: false),
                    IdLista = table.Column<int>(type: "integer", nullable: true),
                    IdCandidato = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votos", x => x.IdVoto);
                    table.ForeignKey(
                        name: "FK_Votos_Candidatos_IdCandidato",
                        column: x => x.IdCandidato,
                        principalTable: "Candidatos",
                        principalColumn: "IdCandidato");
                    table.ForeignKey(
                        name: "FK_Votos_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votos_ListasPoliticas_IdLista",
                        column: x => x.IdLista,
                        principalTable: "ListasPoliticas",
                        principalColumn: "Idlista");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_EleccionIdEleccion",
                table: "Candidatos",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_IdEleccion",
                table: "Candidatos",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_IdLista",
                table: "Candidatos",
                column: "IdLista");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_ListaPoliticaIdlista",
                table: "Candidatos",
                column: "ListaPoliticaIdlista");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPoliticas_EleccionId",
                table: "ListasPoliticas",
                column: "EleccionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_IdEleccion",
                table: "RegistroVotaciones",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_IdUsuario",
                table: "RegistroVotaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_EleccionIdEleccion",
                table: "Resultados",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_IdCandidato",
                table: "Votos",
                column: "IdCandidato");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_IdEleccion",
                table: "Votos",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_IdLista",
                table: "Votos",
                column: "IdLista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroVotaciones");

            migrationBuilder.DropTable(
                name: "Resultados");

            migrationBuilder.DropTable(
                name: "Votos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ListasPoliticas");

            migrationBuilder.DropTable(
                name: "Elecciones");
        }
    }
}
