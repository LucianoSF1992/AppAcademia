using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAcademia.Migrations
{
    /// <inheritdoc />
    public partial class AddDataConclusaoToExercicioConcluido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "ExerciciosConcluidos",
                newName: "DataConclusao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataConclusao",
                table: "ExerciciosConcluidos",
                newName: "Data");
        }
    }
}
