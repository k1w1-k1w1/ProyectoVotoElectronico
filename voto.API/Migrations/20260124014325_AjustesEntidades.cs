using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class AjustesEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo2FA",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoExpira",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "IdCandidato",
                table: "Votos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "IdLista",
                table: "Votos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListaIdlista",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListaPoliticaIdlista",
                table: "Candidatos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegistroVotaciones",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdEleccion = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioIdUsuario = table.Column<int>(type: "integer", nullable: false),
                    EleccionIdEleccion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroVotaciones", x => x.IdRegistro);
                    table.ForeignKey(
                        name: "FK_RegistroVotaciones_Elecciones_EleccionIdEleccion",
                        column: x => x.EleccionIdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroVotaciones_Usuarios_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ListaIdlista",
                table: "Votos",
                column: "ListaIdlista");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_ListaPoliticaIdlista",
                table: "Candidatos",
                column: "ListaPoliticaIdlista");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_EleccionIdEleccion",
                table: "RegistroVotaciones",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_UsuarioIdUsuario",
                table: "RegistroVotaciones",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_ListasPoliticas_ListaPoliticaIdlista",
                table: "Candidatos",
                column: "ListaPoliticaIdlista",
                principalTable: "ListasPoliticas",
                principalColumn: "Idlista");

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_ListasPoliticas_ListaIdlista",
                table: "Votos",
                column: "ListaIdlista",
                principalTable: "ListasPoliticas",
                principalColumn: "Idlista",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_ListasPoliticas_ListaPoliticaIdlista",
                table: "Candidatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_ListasPoliticas_ListaIdlista",
                table: "Votos");

            migrationBuilder.DropTable(
                name: "RegistroVotaciones");

            migrationBuilder.DropIndex(
                name: "IX_Votos_ListaIdlista",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Candidatos_ListaPoliticaIdlista",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "IdLista",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "ListaIdlista",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "ListaPoliticaIdlista",
                table: "Candidatos");

            migrationBuilder.AlterColumn<int>(
                name: "IdCandidato",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo2FA",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoExpira",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
