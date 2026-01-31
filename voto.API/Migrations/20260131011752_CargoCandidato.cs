using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class CargoCandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                table: "Candidatos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdLista",
                table: "Candidatos",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cargo",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "IdLista",
                table: "Candidatos");
        }
    }
}
