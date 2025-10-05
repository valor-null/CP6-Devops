using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public IndexModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    [BindProperty(SupportsGet = true)]
    public int IdConta { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Take { get; set; } = 50;

    [BindProperty(SupportsGet = true)]
    public int Skip { get; set; } = 0;

    public List<TransacaoVm> Transacoes { get; private set; } = new();
    public string? Error { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Error = null;
        Transacoes = new();

        if (IdConta <= 0) return;

        var client = _httpFactory.CreateClient("Api");

        var urls = new[]
        {
            $"/api/transacoes?idConta={IdConta}&take={Take}&skip={Skip}",
            $"/api/Transacoes?idConta={IdConta}&take={Take}&skip={Skip}",
            $"/api/contas/{IdConta}/transacoes?take={Take}&skip={Skip}"
        };

        HttpResponseMessage? last = null;

        foreach (var url in urls)
        {
            try
            {
                var res = await client.GetAsync(url, ct);
                last = res;

                if (res.IsSuccessStatusCode)
                {
                    var list = await res.Content.ReadFromJsonAsync<List<TransacaoVm>>(_json, ct);
                    Transacoes = list ?? new();
                    return;
                }

                if (res.StatusCode == HttpStatusCode.NotFound)
                    continue;
            }
            catch (Exception ex)
            {
                Error = $"Falha ao contatar a API: {ex.Message}";
                return;
            }
        }

        if (last != null)
            Error = $"Falha ao buscar transações (HTTP {(int)last.StatusCode}).";
        else
            Error = "Falha desconhecida ao buscar transações.";
    }

    public class TransacaoVm
    {
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
    }
}
