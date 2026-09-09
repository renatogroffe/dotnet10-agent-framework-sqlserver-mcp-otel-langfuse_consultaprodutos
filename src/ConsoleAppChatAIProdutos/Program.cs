using Azure.AI.OpenAI;
using ConsoleAppChatAIProdutos.Tracing;
using ConsoleAppChatAIProdutos.Utils;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Client;
using OpenAI.Chat;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.ClientModel;
using System.Net.Http.Headers;

Console.WriteLine("***** Testes com Agent Framework + Microsoft Foundry + SQL Server + MCP SQL Server *****");
Console.WriteLine();

var oldForegroundColor = Console.ForegroundColor;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DBProdutos")!;

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService(OpenTelemetryExtensions.ServiceName);

var otlpEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]!;
var otlpHeaders = configuration["OTEL_EXPORTER_OTLP_HEADERS"]!;
var traceProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource(OpenTelemetryExtensions.ServiceName!)
    .AddHttpClientInstrumentation()
    .AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri(otlpEndpoint + "/v1/traces");
        options.Protocol = OtlpExportProtocol.HttpProtobuf;
        options.Headers = otlpHeaders;
    })
    .Build();

string pathDABConfig= Path.Combine(Directory.GetCurrentDirectory(), "dab-config.json");
if (File.Exists(pathDABConfig))
{
    File.Delete(pathDABConfig);
    Console.WriteLine($"{pathDABConfig} removido com sucesso.");
}

CommandLineHelper.Execute($"dab init --database-type mssql --connection-string '{connectionString}'",
    "Inicialização do Data API builder (DAB) CLI - Geração do arquivo arquivo dab-config.json...");

CommandLineHelper.Execute($"dab add Produtos " +
    $"--source dbo.Produtos " +
    $"--source.type table " +
    $"--permissions anonymous:* " +
    $"--description 'Tabela de produtos com código de barras, nome/descrição e preço dos produtos'",
    "Adicionando a tabela de Produtos como entidade via Data API builder (DAB) CLI...");

var mcpName = configuration["MCP:Name"]!;
await using var mcpClient = await McpClient.CreateAsync(new StdioClientTransport(new()
{
    Name = mcpName,
    Command = "dab start",
    Arguments = ["--mcp-stdio", "--config", "./dab-config.json"]
}));
Console.WriteLine($"Ferramentas do MCP:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"***** {mcpName} *****");
var mcpTools = await mcpClient.ListToolsAsync().ConfigureAwait(false);
Console.WriteLine($"Quantidade de ferramentas disponiveis = {mcpTools.Count}");
Console.WriteLine();
foreach (var tool in mcpTools)
{
    Console.WriteLine($"* {tool.Name}: {tool.Description}");
}
Console.ForegroundColor = oldForegroundColor;
Console.WriteLine();


var agent = new AzureOpenAIClient(endpoint: new Uri(configuration["MicrosoftFoundry:Endpoint"]!),
        credential: new ApiKeyCredential(configuration["MicrosoftFoundry:ApiKey"]!))
    .GetChatClient(configuration["MicrosoftFoundry:DeploymentName"]!)
    .AsAIAgent(
        instructions: "Você é um assistente de IA que ajuda o usuario a consultar informações" +
            "sobre produtos em uma base de dados do SQL Server.",
        tools: [.. mcpTools])
    .AsBuilder()
    .UseOpenTelemetry(sourceName: OpenTelemetryExtensions.ServiceName)
    .Build();

while (true)
{
    Console.WriteLine("Sua pergunta:");
    var userPrompt = Console.ReadLine();

    using var activity1 = OpenTelemetryExtensions.ActivitySource
        .StartActivity("PerguntaChatIAProdutos")!;

    var result = await agent.RunAsync(userPrompt!);

    Console.WriteLine();
    Console.WriteLine("Resposta da IA:");
    Console.WriteLine();

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(result.AsChatResponse().Messages.Last().Text);
    Console.ForegroundColor = oldForegroundColor;

    Console.WriteLine();
    Console.WriteLine();

    activity1.Stop();
}