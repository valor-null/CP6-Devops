using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    [BindProperty(SupportsGet = true)]
    public int? IdConta { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Take { get; set; } = 50;

    [BindProperty(SupportsGet = true)]
    public int Skip { get; set; } = 0;

    public List<TransacaoVm> Itens { get; private set; } = new();

    public IndexModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (IdConta is null || IdConta <= 0) return;

        var client = _httpFactory.CreateClient("Api");
        using var res = await client.GetAsync($"/api/transacoes/conta/{IdConta}?take={Take}&skip={Skip}", ct);
        res.EnsureSuccessStatusCode();
        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<TransacaoVm>>(stream, _json, ct);
        Itens = data ?? new List<TransacaoVm>();
    }

    public record TransacaoVm
    {
        public int IdTransacao { get; set; }
        public int IdConta { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
    }
}