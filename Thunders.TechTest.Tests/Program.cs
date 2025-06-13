using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Models;

Console.WriteLine("Iniciando teste de carga...");

ProcessamentoRelatoriosAsync();

var client = new HttpClient();
client.Timeout = TimeSpan.FromSeconds(300);
client.BaseAddress = new Uri("https://localhost:7405/api/PedagioUtilizacao/");


int totalRequests = 500;
int requestsPage = 50;
int totalRequestsInd = 50;
var random = new Random();
var cache = 0;
var millisecondsTimeout = 1000;

Console.WriteLine("Para iniciar o envio individual aperte I, ou qualqer outra tecla para envio em lote");

var read = Console.ReadKey().KeyChar.ToString().ToUpper();

while (read != "I") {
    for (int i = 0; i < totalRequests; i++)
    {
        if (cache >= totalRequests)
            continue;

        cache += requestsPage;

        EnviarRequisicaoListAsync(client, GerarPedagioUtilizacaoAleatorioList(random));

        Thread.Sleep(1000);
    }
}


while (read == "I")
{
    for (int i = 0; i < totalRequestsInd; i++)
    {
        if (cache >= totalRequestsInd)
            continue;

        cache += 1;

        var dto = GerarPedagioUtilizacaoAleatorio(random, i);
        EnviarRequisicaoAsync(client, dto);
    }

    Thread.Sleep(5000);
}

List<PedagioUtilizacaoCreateRequestDTOSeq> GerarPedagioUtilizacaoAleatorioList(Random random) {
    var list = new List<PedagioUtilizacaoCreateRequestDTOSeq>();

    var qtdPadrao = 0;
    for (int i = 0; i < requestsPage; i++) {
        qtdPadrao += 1;

        if (qtdPadrao == 2) {
            qtdPadrao = 0;
            list.Add(GerarPedagioUtilizacaoAleatorio(random, i));
        }
        else
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

async Task EnviarRequisicaoListAsync(HttpClient client, IList<PedagioUtilizacaoCreateRequestDTOSeq> dto)
{
    try
    {
        var inicio = DateTime.Now;
        Console.WriteLine($"Iniciando envio de {dto.Count} itens. {inicio}");
        var response = await client.PostAsJsonAsync("range", dto);
        var fim = DateTime.Now;
        if (response.IsSuccessStatusCode)
            Console.WriteLine($"Sucesso. Tempo: {fim-inicio}");
        else
            Console.WriteLine($"Falha");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

    cache -= requestsPage;
}

// Novo método para chamada ao outro endpoint
async Task ProcessamentoRelatoriosAsync()
{
    var client = new HttpClient();
    client.Timeout = TimeSpan.FromMinutes(5);
    client.BaseAddress = new Uri("https://localhost:7405/api/PedagioUtilizacao/");

    while (true) { 
        try
        {
            var inicio = DateTime.Now;

            ConsoleColor corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[RelatorioResumo] Inicio: {DateTime.Now}");
            Console.ForegroundColor = corOriginal;

            var response = await client.PostAsync("ProcessamentoRelatorios", new StringContent(""));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var processamentoRelatoriosDTO = JsonSerializer.Deserialize<ProcessamentoRelatoriosDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                corOriginal = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[RelatorioResumo] Sucesso - Tempo: {(DateTime.Now - inicio).TotalSeconds} segundos");

                Console.WriteLine($"FaturamentoPracaMesReportList: {processamentoRelatoriosDTO.FaturamentoPracaMesReportList?.Count() ?? 0} registros");
                Console.WriteLine($"FaturamentoHoraCidadeReportList: {processamentoRelatoriosDTO.FaturamentoHoraCidadeReportList?.Count() ?? 0} registros");
                Console.WriteLine($"FaturamentoPracaTipoVeiculoReportList :{processamentoRelatoriosDTO.FaturamentoPracaTipoVeiculoReportList?.Count() ?? 0} registros");

                Console.ForegroundColor = corOriginal;


                //if (content.Length <= 116)
                //    Thread.Sleep(30000);
            }
            else
            {
                corOriginal = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[RelatorioResumo] Falha: {response.StatusCode}");
                Console.ForegroundColor = corOriginal;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RelatorioResumo] Erro: {ex.Message}");
        }

        Thread.Sleep(30000);
    }
}


public class PedagioUtilizacaoCreateRequestDTOSeq : PedagioUtilizacaoCreateRequestDTO { 
    public int Index { private get; set; }
    public int GetIndex() {
        return Index;
    }
}