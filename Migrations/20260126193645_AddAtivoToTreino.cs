using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAcademia.Migrations
{
    /// <inheritdoc />
    public partial class AddAtivoToTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Treinos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Treinos");
        }
    }
}
