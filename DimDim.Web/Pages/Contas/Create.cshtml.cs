using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Contas;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public CreateModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    [TempData]
    public string? Success { get; set; }

    [BindProperty]
    public ContaForm Form { get; set; } = new();

    public List<ClienteItem> Clientes { get; private set; } = new();

    public async Task OnGetAsync(CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("Api");
        using var res = await client.GetAsync("/api/clientes", ct);
        res.EnsureSuccessStatusCode();
        var list = await res.Content.ReadFromJsonAsync<List<ClienteItem>>(_json, ct);
        Clientes = list ?? new();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(ct);
            return Page();
        }

        var client = _httpFactory.CreateClient("Api");
        var res = await client.PostAsJsonAsync("/api/contas", new
        {
            idCliente = Form.IdCliente,
            numeroConta = Form.NumeroConta,
            tipoConta = Form.TipoConta
        }, ct);

        if (res.IsSuccessStatusCode)
        {
            Success = "Conta criada com sucesso.";
            return RedirectToPage("/Contas/Index");
        }

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha ao criar conta." : msg);
        await OnGetAsync(ct);
        return Page();
    }

    public class ContaForm
    {
        [Range(1, int.MaxValue)]
        public int IdCliente { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string NumeroConta { get; set; } = string.Empty;

        [Required, RegularExpression("^(Corrente|Poupanca)$")]
        public string TipoConta { get; set; } = "Corrente";
    }

    public class ClienteItem
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
