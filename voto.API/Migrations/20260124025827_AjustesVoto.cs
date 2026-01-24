using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class AjustesVoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                table: "Candidatos");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroVotaciones_Elecciones_EleccionIdEleccion",
                table: "RegistroVotaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroVotaciones_Usuarios_UsuarioIdUsuario",
                table: "RegistroVotaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Resultados_Elecciones_EleccionIdEleccion",
                table: "Resultados");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Candidatos_CandidatoIdCandidato",
                table: "Votos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Elecciones_EleccionIdEleccion",
                table: "Votos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_ListasPoliticas_ListaIdlista",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_CandidatoIdCandidato",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_EleccionIdEleccion",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_ListaIdlista",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_RegistroVotaciones_EleccionIdEleccion",
                table: "RegistroVotaciones");

            migrationBuilder.DropIndex(
                name: "IX_RegistroVotaciones_UsuarioIdUsuario",
                table: "RegistroVotaciones");

            migrationBuilder.DropColumn(
                name: "CandidatoIdCandidato",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "EleccionIdEleccion",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "ListaIdlista",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "EleccionIdEleccion",
                table: "RegistroVotaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "RegistroVotaciones");

            migrationBuilder.AlterColumn<string>(
                name: "HashVoto",
                table: "Votos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "Votos",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Roles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Roles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MetodoAsignacion",
                table: "Resultados",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCalculo",
                table: "Resultados",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "EleccionIdEleccion",
                table: "Resultados",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "RegistroVotaciones",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "NombreLista",
                table: "ListasPoliticas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Elecciones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Elecciones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Elecciones",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFin",
                table: "Elecciones",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Elecciones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Elecciones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Propuesta",
                table: "Candidatos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Candidatos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FotoUrl",
                table: "Candidatos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "EleccionIdEleccion",
                table: "Candidatos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Candidatos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_IdEleccion",
                table: "RegistroVotaciones",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_IdUsuario",
                table: "RegistroVotaciones",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                table: "Candidatos",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroVotaciones_Elecciones_IdEleccion",
                table: "RegistroVotaciones",
                column: "IdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroVotaciones_Usuarios_IdUsuario",
                table: "RegistroVotaciones",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resultados_Elecciones_EleccionIdEleccion",
                table: "Resultados",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion");

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Candidatos_IdCandidato",
                table: "Votos",
                column: "IdCandidato",
                principalTable: "Candidatos",
                principalColumn: "IdCandidato");

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Elecciones_IdEleccion",
                table: "Votos",
                column: "IdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_ListasPoliticas_IdLista",
                table: "Votos",
                column: "IdLista",
                principalTable: "ListasPoliticas",
                principalColumn: "Idlista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                table: "Candidatos");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroVotaciones_Elecciones_IdEleccion",
                table: "RegistroVotaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroVotaciones_Usuarios_IdUsuario",
                table: "RegistroVotaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Resultados_Elecciones_EleccionIdEleccion",
                table: "Resultados");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Candidatos_IdCandidato",
                table: "Votos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Elecciones_IdEleccion",
                table: "Votos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_ListasPoliticas_IdLista",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_IdCandidato",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_IdEleccion",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_IdLista",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_RegistroVotaciones_IdEleccion",
                table: "RegistroVotaciones");

            migrationBuilder.DropIndex(
                name: "IX_RegistroVotaciones_IdUsuario",
                table: "RegistroVotaciones");

            migrationBuilder.AlterColumn<string>(
                name: "HashVoto",
                table: "Votos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "Votos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "CandidatoIdCandidato",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EleccionIdEleccion",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListaIdlista",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetodoAsignacion",
                table: "Resultados",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCalculo",
                table: "Resultados",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "EleccionIdEleccion",
                table: "Resultados",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "RegistroVotaciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "EleccionIdEleccion",
                table: "RegistroVotaciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "RegistroVotaciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "NombreLista",
                table: "ListasPoliticas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Elecciones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Elecciones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Elecciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFin",
                table: "Elecciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Elecciones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Elecciones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Propuesta",
                table: "Candidatos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Candidatos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FotoUrl",
                table: "Candidatos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EleccionIdEleccion",
                table: "Candidatos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Candidatos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CandidatoIdCandidato",
                table: "Votos",
                column: "CandidatoIdCandidato");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_EleccionIdEleccion",
                table: "Votos",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ListaIdlista",
                table: "Votos",
                column: "ListaIdlista");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_EleccionIdEleccion",
                table: "RegistroVotaciones",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroVotaciones_UsuarioIdUsuario",
                table: "RegistroVotaciones",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                table: "Candidatos",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroVotaciones_Elecciones_EleccionIdEleccion",
                table: "RegistroVotaciones",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroVotaciones_Usuarios_UsuarioIdUsuario",
                table: "RegistroVotaciones",
                column: "UsuarioIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resultados_Elecciones_EleccionIdEleccion",
                table: "Resultados",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Candidatos_CandidatoIdCandidato",
                table: "Votos",
                column: "CandidatoIdCandidato",
                principalTable: "Candidatos",
                principalColumn: "IdCandidato",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Elecciones_EleccionIdEleccion",
                table: "Votos",
                column: "EleccionIdEleccion",
                principalTable: "Elecciones",
                principalColumn: "IdEleccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_ListasPoliticas_ListaIdlista",
                table: "Votos",
                column: "ListaIdlista",
                principalTable: "ListasPoliticas",
                principalColumn: "Idlista",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
