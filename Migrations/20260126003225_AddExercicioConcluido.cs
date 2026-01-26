using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAcademia.Migrations
{
    /// <inheritdoc />
    public partial class AddExercicioConcluido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciciosConcluidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExercicioId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciciosConcluidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciciosConcluidos_Exercicios_ExercicioId",
                        column: x => x.ExercicioId,
                        principalTable: "Exercicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciciosConcluidos_ExercicioId",
                table: "ExerciciosConcluidos",
                column: "ExercicioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciciosConcluidos");
        }
    }
}
