namespace Thunders.TechTest.ApiService.DTOs
{
    public class ProcessamentoRelatoriosDTO
    {
        public IList<FaturamentoHoraCidadeReportDTO> FaturamentoHoraCidadeReportList { get; set; }
        public IList<FaturamentoPracaMesReportDTO> FaturamentoPracaMesReportList { get; set; }
        public IList<FaturamentoPracaTipoVeiculoReportDTO> FaturamentoPracaTipoVeiculoReportList { get; set; }
    }
}
