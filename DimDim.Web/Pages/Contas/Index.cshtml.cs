using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Contas;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public IndexModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    public List<ContaVm> Contas { get; private set; } = new();
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            var client = _httpFactory.CreateClient("Api");
            using var res = await client.GetAsync("/api/contas", ct);
            res.EnsureSuccessStatusCode();
            await using var stream = await res.Content.ReadAsStreamAsync(ct);
            var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<ContaVm>>(stream, _json, ct);
            Contas = data ?? new List<ContaVm>();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Falha ao contatar a API: {ex.Message}";
            Contas = new List<ContaVm>();
        }
    }

    public record ContaVm
    {
        public int IdConta { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public int IdCliente { get; set; }
    }
}