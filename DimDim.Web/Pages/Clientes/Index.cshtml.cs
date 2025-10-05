using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public List<ClienteVm> Clientes { get; private set; } = new();

    public IndexModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("Api");
        using var res = await client.GetAsync("/api/clientes", ct);
        res.EnsureSuccessStatusCode();
        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<ClienteVm>>(stream, _json, ct);
        Clientes = data ?? new List<ClienteVm>();
    }

    public record ClienteVm
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}