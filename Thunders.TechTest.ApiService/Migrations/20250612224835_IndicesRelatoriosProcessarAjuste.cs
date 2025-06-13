using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class IndicesRelatoriosProcessarAjuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_PedagioUtilizacao_VeiculoId",
                table: "PedagiosUtilizacoes",
                newName: "IX_PedagiosUtilizacoes_VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "VeiculoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PedagioUtilizacao_PracaId_VeiculoId",
                table: "PedagiosUtilizacoes");

            migrationBuilder.RenameIndex(
                name: "IX_PedagiosUtilizacoes_VeiculoId",
                table: "PedagiosUtilizacoes",
                newName: "IX_PedagioUtilizacao_VeiculoId");
        }
    }
}
