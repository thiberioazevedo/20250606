using System.Net.Http.Json;
using System.Text.Json;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Models;

Console.WriteLine("Iniciando teste de carga...");

IniciarProcessamentoRelatoriosEmSegundoPlano();

var client = CriarHttpClient();
var cache = 0;

ExecutarTesteCarga(client);

HttpClient CriarHttpClient()
{
    var client = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(300),
        BaseAddress = new Uri("https://localhost:7405/api/PedagioUtilizacao/")
    };
    return client;
}

void ExecutarTesteCarga(HttpClient client)
{
    const int totalRequests = 500;
    const int requestsPage = 50;
    const int totalRequestsInd = 50;
    var random = new Random();

    Console.WriteLine("Para iniciar o envio individual aperte I, ou qualquer outra tecla para envio em lote");

    var read = Console.ReadKey().KeyChar.ToString().ToUpper();

    while (true) { 
        if (read != "I")
            EnvioEmLote(client, random, totalRequests, requestsPage);
        else
            EnvioIndividual(client, random, totalRequestsInd);
    }
}

void EnvioEmLote(HttpClient client, Random random, int totalRequests, int requestsPage)
{
    for (int i = 0; i < totalRequests; i++)
    {
        if (cache >= totalRequests)
            continue;

        cache += requestsPage;

        var lista = GerarPedagioUtilizacaoAleatorioList(random, requestsPage);
        _ = EnviarRequisicaoListAsync(client, lista, requestsPage);

        Thread.Sleep(1000);
    }
}

void EnvioIndividual(HttpClient client, Random random, int totalRequestsInd)
{
    for (int i = 0; i < totalRequestsInd; i++)
    {
        if (cache >= totalRequestsInd)
            continue;

        cache += 1;
        var dto = GerarPedagioUtilizacaoAleatorio(random, i);
        _ = EnviarRequisicaoAsync(client, dto);
    }

    Thread.Sleep(5000);
}

List<PedagioUtilizacaoCreateRequestDTOSeq> GerarPedagioUtilizacaoAleatorioList(Random random, int requestsPage)
{
    var list = new List<PedagioUtilizacaoCreateRequestDTOSeq>();
    var qtdPadrao = 0;
    for (int i = 0; i < requestsPage; i++)
    {
        qtdPadrao++;
        if (qtdPadrao == 2)
        {
            qtdPadrao = 0;
            list.Add(GerarPedagioUtilizacaoAleatorio(random, i));
        }
        else
        {
            list.Add(new PedagioUtilizacaoCreateRequestDTOSeq
            {
                Cidade = "Maracanaú",
                Placa = "ABC1230",
                DataHoraUtilizacao = DateTime.Now,
                Index = i,
                Praca = "Praça A",
                TipoVeiculo = ETipoVeiculo.Carro,
                UF = "CE",
                ValorPago = Math.Round((decimal)(random.NextDouble() * 490 + 10), 2)
            });
        }
    }
    return list;
}

PedagioUtilizacaoCreateRequestDTOSeq GerarPedagioUtilizacaoAleatorio(Random random, int index)
{
    var dataHora = DateTime.UtcNow.AddMinutes(-random.Next(0, 60 * 24 * 30));
    var pracas = new[] { "Praça A", "Praça B", "Praça C" };
    var cidades = new[] { "Cidade X", "Cidade Y", "Cidade Z" };
    var ufs = new[] { "SP", "RJ", "MG" };
    var tiposVeiculo = Enum.GetValues(typeof(ETipoVeiculo));
    var tipoVeiculo = (ETipoVeiculo)tiposVeiculo.GetValue(random.Next(tiposVeiculo.Length));
    decimal valorPago = Math.Round((decimal)(random.NextDouble() * 490 + 10), 2);
    string placa = $"{RandomLetras(random, 3)}{random.Next(1000, 9999)}";

    return new PedagioUtilizacaoCreateRequestDTOSeq
    {
        Index = index,
        DataHoraUtilizacao = dataHora,
        Praca = pracas[random.Next(pracas.Length)],
        Cidade = cidades[random.Next(cidades.Length)],
        UF = ufs[random.Next(ufs.Length)],
        ValorPago = valorPago,
        TipoVeiculo = tipoVeiculo,
        Placa = placa
    };
}

string RandomLetras(Random random, int count)
{
    const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    return new string(Enumerable.Range(0, count).Select(_ => letras[random.Next(letras.Length)]).ToArray());
}

async Task EnviarRequisicaoAsync(HttpClient client, PedagioUtilizacaoCreateRequestDTOSeq dto)
{
    try
    {
        var response = await client.PostAsJsonAsync("", dto);
        if (response.IsSuccessStatusCode)
            Console.WriteLine($"Sucesso: {dto.GetIndex()} - {dto.Praca} - {dto.Placa} - {dto.ValorPago:C}");
        else
            Console.WriteLine($"Falha: {dto.GetIndex()} - {response.StatusCode} - {dto.Praca} - {dto.Placa}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {dto.GetIndex()} - {ex.Message}");
    }
    cache -= 1;
}

async Task EnviarRequisicaoListAsync(HttpClient client, IList<PedagioUtilizacaoCreateRequestDTOSeq> dto, int requestsPage)
{
    try
    {
        var inicio = DateTime.Now;
        Console.WriteLine($"Iniciando envio de {dto.Count} itens. {inicio}");
        var response = await client.PostAsJsonAsync("range", dto);
        var fim = DateTime.Now;
        if (response.IsSuccessStatusCode)
            Console.WriteLine($"Sucesso. Tempo: {fim - inicio}");
        else
            Console.WriteLine($"Falha");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
    cache -= requestsPage;
}

void IniciarProcessamentoRelatoriosEmSegundoPlano()
{
    _ = Task.Run(ProcessamentoRelatoriosAsync);
}

async Task ProcessamentoRelatoriosAsync()
{
    var client = new HttpClient
    {
        Timeout = TimeSpan.FromMinutes(5),
        BaseAddress = new Uri("https://localhost:7405/api/PedagioUtilizacao/")
    };

    while (true)
    {
        try
        {
            var inicio = DateTime.Now;
            ExibirMensagem("[RelatorioResumo] Inicio: " + DateTime.Now, ConsoleColor.Yellow);

            var response = await client.PostAsync("ProcessamentoRelatorios", new StringContent(""));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var processamentoRelatoriosDTO = JsonSerializer.Deserialize<ProcessamentoRelatoriosDTO>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                ExibirMensagem($"[RelatorioResumo] Sucesso - Tempo: {(DateTime.Now - inicio).TotalSeconds} segundos", ConsoleColor.Green);
                Console.WriteLine($"FaturamentoPracaMesReportList: {processamentoRelatoriosDTO?.FaturamentoPracaMesReportList?.Count() ?? 0} registros");
                Console.WriteLine($"FaturamentoHoraCidadeReportList: {processamentoRelatoriosDTO?.FaturamentoHoraCidadeReportList?.Count() ?? 0} registros");
                Console.WriteLine($"FaturamentoPracaTipoVeiculoReportList: {processamentoRelatoriosDTO?.FaturamentoPracaTipoVeiculoReportList?.Count() ?? 0} registros");
            }
            else
            {
                ExibirMensagem($"[RelatorioResumo] Falha: {response.StatusCode}", ConsoleColor.Red);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RelatorioResumo] Erro: {ex.Message}");
        }

        Thread.Sleep(30000);
    }
}

void ExibirMensagem(string mensagem, ConsoleColor cor)
{
    var corOriginal = Console.ForegroundColor;
    Console.ForegroundColor = cor;
    Console.WriteLine(mensagem);
    Console.ForegroundColor = corOriginal;
}

public class PedagioUtilizacaoCreateRequestDTOSeq : PedagioUtilizacaoCreateRequestDTO
{
    public int Index { get; set; }
    public int GetIndex() => Index;
}