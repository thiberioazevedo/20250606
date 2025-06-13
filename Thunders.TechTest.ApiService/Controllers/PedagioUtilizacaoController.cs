using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedagioUtilizacaoController : ControllerBase
    {
        private readonly IPedagioUtilizacaoService pedagioUtilizacaoService;

        public PedagioUtilizacaoController(IPedagioUtilizacaoService pedagioUtilizacaoService)
        {
            this.pedagioUtilizacaoService = pedagioUtilizacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedagioUtilizacaoCreateRequestDTO pedagioUtilizacaoCreateRequestDTO, CancellationToken cancellationToken)
        {
            var pedagioUtilizacao = await pedagioUtilizacaoService.CreatePedagioUtilizacaoAsync(pedagioUtilizacaoCreateRequestDTO, cancellationToken);

            return Created(string.Empty, pedagioUtilizacao);
        }

        [HttpGet()]
        public async Task<IActionResult> List([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var entity = await pedagioUtilizacaoService.GetPaginatedList(pageNumber, pageSize, cancellationToken);
            
            return Ok(entity);
        }


        [HttpPost("range")]
        public async Task<IActionResult> PostRange(
             [FromBody] IList<PedagioUtilizacaoCreateRequestDTO> pedagioUtilizacaoCreateRequestDTOList, CancellationToken cancellationToken)
        {
            var pedagioUtilizacaoDTOList = await pedagioUtilizacaoService.CreatePedagioUtilizacaoRangeAsync(pedagioUtilizacaoCreateRequestDTOList, cancellationToken);

            return Created(string.Empty, pedagioUtilizacaoDTOList);
        }

        [HttpPost("ProcessamentoRelatorios")]
        public async Task<IActionResult> ProcessamentoRelatorios(CancellationToken cancellationToken)
        {
            var processamentoRelatoriosDTO = await pedagioUtilizacaoService.ProcessamentoRelatoriosAsync(cancellationToken);

            return Ok(processamentoRelatoriosDTO);
        }
    }
}
