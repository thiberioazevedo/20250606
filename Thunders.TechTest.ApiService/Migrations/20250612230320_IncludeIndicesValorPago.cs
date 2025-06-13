using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class IncludeIndicesValorPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao",
                table: "PedagiosUtilizacoes");

            migrationBuilder.DropIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes");

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "DataHoraUtilizacao" })
                .Annotation("SqlServer:Include", new[] { "ValorPago" });

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "VeiculoId" })
                .Annotation("SqlServer:Include", new[] { "ValorPago" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao",
                table: "PedagiosUtilizacoes");

            migrationBuilder.DropIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes");

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "DataHoraUtilizacao" });

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "VeiculoId" });
        }
    }
}
