using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class IndicesRelatoriosProcessar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosPracasTiposVeiculosReports_Processar",
                table: "FaturamentosPracasTiposVeiculosReports",
                column: "Processar")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosPracasMesesReports_Processar",
                table: "FaturamentosPracasMesesReports",
                column: "Processar")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosHorasCidadesReports_Processar",
                table: "FaturamentosHorasCidadesReports",
                column: "Processar")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FaturamentosPracasTiposVeiculosReports_Processar",
                table: "FaturamentosPracasTiposVeiculosReports");

            migrationBuilder.DropIndex(
                name: "IX_FaturamentosPracasMesesReports_Processar",
                table: "FaturamentosPracasMesesReports");

            migrationBuilder.DropIndex(
                name: "IX_FaturamentosHorasCidadesReports_Processar",
                table: "FaturamentosHorasCidadesReports");
        }
    }
}
