using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viamatica.Blog.Api.Migrations
{
    /// <inheritdoc />
    public partial class serealizalamejoradelatablapublicacionyaquesenecesitasolamenteguardarelusuariologueadoynecesitaunstringsimplemente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publicaciones_Usuarios_UsuarioId",
                table: "Publicaciones");

            migrationBuilder.DropIndex(
                name: "IX_Publicaciones_UsuarioId",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Publicaciones");

            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "Publicaciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "Publicaciones");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Publicaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_UsuarioId",
                table: "Publicaciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publicaciones_Usuarios_UsuarioId",
                table: "Publicaciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
