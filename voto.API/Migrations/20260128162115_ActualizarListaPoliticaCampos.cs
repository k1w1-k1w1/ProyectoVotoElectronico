using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarListaPoliticaCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ListasPoliticas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlLogo",
                table: "ListasPoliticas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ListasPoliticas");

            migrationBuilder.DropColumn(
                name: "UrlLogo",
                table: "ListasPoliticas");
        }
    }
}
