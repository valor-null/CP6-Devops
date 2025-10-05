using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Contas;

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

    public ContaVm? Conta { get; private set; }

    [BindProperty]
    public bool Confirm { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Contas/Index");

        var client = _httpFactory.CreateClient("Api");
        var res = await client.GetAsync($"/api/contas/{Id}", ct);
        if (!res.IsSuccessStatusCode) return RedirectToPage("/Contas/Index");

        Conta = await res.Content.ReadFromJsonAsync<ContaVm>(_json, ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0 || !Confirm) return RedirectToPage("/Contas/Index");

        var client = _httpFactory.CreateClient("Api");
        var res = await client.DeleteAsync($"/api/contas/{Id}", ct);

        if (res.IsSuccessStatusCode)
        {
            Success = "Conta excluída com sucesso.";
            return RedirectToPage("/Contas/Index");
        }

        var msg = await res.Content.ReadAsStringAsync(ct);
        Error = string.IsNullOrWhiteSpace(msg) ? "Não foi possível excluir a conta." : msg;
        return RedirectToPage("/Contas/Index");
    }

    public class ContaVm
    {
        public int IdConta { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public int IdCliente { get; set; }
    }
}
