using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AjusteIndiceVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculo_TipoVeiculo",
                table: "Veiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_TipoVeiculo_Id",
                table: "Veiculos",
                columns: new[] { "TipoVeiculo", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculo_TipoVeiculo_Id",
                table: "Veiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_TipoVeiculo",
                table: "Veiculos",
                column: "TipoVeiculo");
        }
    }
}
