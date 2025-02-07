using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackaton.Infra.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaoDeaceitarAgendaaamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Consulta",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Consulta");
        }
    }
}
