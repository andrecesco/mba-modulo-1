using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLV.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MudancaDeNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemNome",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoImagem",
                table: "Produtos",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoImagem",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "ImagemNome",
                table: "Produtos",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
