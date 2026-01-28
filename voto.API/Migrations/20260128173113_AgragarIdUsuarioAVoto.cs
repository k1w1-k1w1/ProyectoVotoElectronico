using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class AgragarIdUsuarioAVoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Votos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Votos");
        }
    }
}
