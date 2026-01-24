using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voto.API.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUserIdUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Usuarios",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Usuarios");
        }
    }
}
