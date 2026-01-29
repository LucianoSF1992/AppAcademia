using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAcademia.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirDiaSemanaEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Criar coluna temporária INT
            migrationBuilder.AddColumn<int>(
                name: "DiaSemanaInt",
                table: "Treinos",
                nullable: false,
                defaultValue: 0);

            // 2️⃣ Converter valores texto → int
            migrationBuilder.Sql(@"
        UPDATE Treinos SET DiaSemanaInt = 1 WHERE DiaSemana = 'Segunda';
        UPDATE Treinos SET DiaSemanaInt = 2 WHERE DiaSemana = 'Terca';
        UPDATE Treinos SET DiaSemanaInt = 3 WHERE DiaSemana = 'Quarta';
        UPDATE Treinos SET DiaSemanaInt = 4 WHERE DiaSemana = 'Quinta';
        UPDATE Treinos SET DiaSemanaInt = 5 WHERE DiaSemana = 'Sexta';
        UPDATE Treinos SET DiaSemanaInt = 6 WHERE DiaSemana = 'Sabado';
        UPDATE Treinos SET DiaSemanaInt = 0 WHERE DiaSemana = 'Domingo';
    ");

            // 3️⃣ Remover coluna antiga
            migrationBuilder.DropColumn(
                name: "DiaSemana",
                table: "Treinos");

            // 4️⃣ Renomear coluna nova
            migrationBuilder.RenameColumn(
                name: "DiaSemanaInt",
                table: "Treinos",
                newName: "DiaSemana");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaSemanaTexto",
                table: "Treinos",
                nullable: true);

            migrationBuilder.Sql(@"
        UPDATE Treinos SET DiaSemanaTexto = 
        CASE DiaSemana
            WHEN 0 THEN 'Domingo'
            WHEN 1 THEN 'Segunda'
            WHEN 2 THEN 'Terca'
            WHEN 3 THEN 'Quarta'
            WHEN 4 THEN 'Quinta'
            WHEN 5 THEN 'Sexta'
            WHEN 6 THEN 'Sabado'
        END
    ");

            migrationBuilder.DropColumn(
                name: "DiaSemana",
                table: "Treinos");

            migrationBuilder.RenameColumn(
                name: "DiaSemanaTexto",
                table: "Treinos",
                newName: "DiaSemana");
        }
    }
}
