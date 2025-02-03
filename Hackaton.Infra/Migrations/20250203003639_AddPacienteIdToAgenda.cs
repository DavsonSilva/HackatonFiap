using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackaton.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteIdToAgenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Agenda",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_PacienteId",
                table: "Agenda",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agenda_Usuario_PacienteId",
                table: "Agenda",
                column: "PacienteId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agenda_Usuario_PacienteId",
                table: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Agenda_PacienteId",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Agenda");
        }
    }
}
