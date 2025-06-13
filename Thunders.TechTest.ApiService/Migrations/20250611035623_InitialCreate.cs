using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UF = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Placa = table.Column<string>(type: "VARCHAR(7)", nullable: false),
                    TipoVeiculo = table.Column<int>(type: "int", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cidades_Estados",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaturamentosHorasCidadesReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "DATE", nullable: false),
                    Hora = table.Column<short>(type: "smallint", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InicioProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FimProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processar = table.Column<bool>(type: "bit", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentosHorasCidadesReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturamentosHorasCidadesReports_Cidades",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pracas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    CidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pracas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cidades_Pracas",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaturamentosPracasMesesReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PracaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ano = table.Column<short>(type: "smallint", nullable: false),
                    Mes = table.Column<short>(type: "smallint", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InicioProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FimProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processar = table.Column<bool>(type: "bit", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentosPracasMesesReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturamentosPracasMesesReports_Pracas",
                        column: x => x.PracaId,
                        principalTable: "Pracas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaturamentosPracasTiposVeiculosReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PracaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoVeiculo = table.Column<int>(type: "int", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InicioProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FimProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processar = table.Column<bool>(type: "bit", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentosPracasTiposVeiculosReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturamentosPracasTiposVeiculosReports_Pracas",
                        column: x => x.PracaId,
                        principalTable: "Pracas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedagiosUtilizacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataHoraUtilizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PracaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorPago = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    Sequencial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedagiosUtilizacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedagiosUtilizacoes_Pracas",
                        column: x => x.PracaId,
                        principalTable: "Pracas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedagiosUtilizacoes_Veiculos",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosHorasCidadesReports_CidadeId",
                table: "FaturamentosHorasCidadesReports",
                column: "CidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosPracasMesesReports_PracaId",
                table: "FaturamentosPracasMesesReports",
                column: "PracaId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentosPracasTiposVeiculosReports_PracaId",
                table: "FaturamentosPracasTiposVeiculosReports",
                column: "PracaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao",
                table: "PedagiosUtilizacoes",
                columns: new[] { "PracaId", "DataHoraUtilizacao" });

            migrationBuilder.CreateIndex(
                name: "IX_PedagioUtilizacao_VeiculoId",
                table: "PedagiosUtilizacoes",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Praca_CidadeId",
                table: "Pracas",
                column: "CidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_TipoVeiculo",
                table: "Veiculos",
                column: "TipoVeiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaturamentosHorasCidadesReports");

            migrationBuilder.DropTable(
                name: "FaturamentosPracasMesesReports");

            migrationBuilder.DropTable(
                name: "FaturamentosPracasTiposVeiculosReports");

            migrationBuilder.DropTable(
                name: "PedagiosUtilizacoes");

            migrationBuilder.DropTable(
                name: "Pracas");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "Estados");
        }
    }
}
