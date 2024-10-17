using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class segundaMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorH",
                table: "Folhas",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "QuantH",
                table: "Folhas",
                newName: "Quantidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Folhas",
                newName: "ValorH");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Folhas",
                newName: "QuantH");
        }
    }
}
