using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLV.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemNome",
                table: "Produtos",
                type: "varchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemNome",
                table: "Produtos");
        }
    }
}
