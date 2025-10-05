using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public DeleteModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    [TempData]
    public string? Success { get; set; }

    [TempData]
    public string? Error { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public ClienteVm? Cliente { get; private set; }

    [BindProperty]
    public bool Confirm { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Clientes/Index");

        var client = _httpFactory.CreateClient("Api");
        var res = await client.GetAsync($"/api/clientes/{Id}", ct);
        if (!res.IsSuccessStatusCode) return RedirectToPage("/Clientes/Index");

        Cliente = await res.Content.ReadFromJsonAsync<ClienteVm>(_json, ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0 || !Confirm) return RedirectToPage("/Clientes/Index");

        var client = _httpFactory.CreateClient("Api");
        var res = await client.DeleteAsync($"/api/clientes/{Id}", ct);

        if (res.IsSuccessStatusCode)
        {
            Success = "Cliente excluído com sucesso.";
            return RedirectToPage("/Clientes/Index");
        }

        var msg = await res.Content.ReadAsStringAsync(ct);
        Error = string.IsNullOrWhiteSpace(msg) ? "Não foi possível excluir o cliente." : msg;
        return RedirectToPage("/Clientes/Index");
    }

    public class ClienteVm
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}
