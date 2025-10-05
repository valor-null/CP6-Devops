using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Contas;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public EditModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    [TempData]
    public string? Success { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public ContaForm Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Contas/Index");

        var client = _httpFactory.CreateClient("Api");
        var res = await client.GetAsync($"/api/contas/{Id}", ct);
        if (!res.IsSuccessStatusCode) return RedirectToPage("/Contas/Index");

        var vm = await res.Content.ReadFromJsonAsync<ContaVm>(_json, ct);
        if (vm == null) return RedirectToPage("/Contas/Index");

        Form.NumeroConta = vm.NumeroConta;
        Form.TipoConta = vm.TipoConta;
        Form.IdCliente = vm.IdCliente;
        Form.Saldo = vm.Saldo;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Contas/Index");
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("Api");
        var res = await client.PutAsJsonAsync($"/api/contas/{Id}/tipo", new { tipoConta = Form.TipoConta }, _json, ct);

        if (res.IsSuccessStatusCode)
        {
            Success = "Conta atualizada com sucesso.";
            return RedirectToPage("/Contas/Index");
        }

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha ao atualizar conta." : msg);
        return Page();
    }

    public class ContaForm
    {
        [Display(Name = "Número da Conta")]
        public string NumeroConta { get; set; } = string.Empty;

        [Required, RegularExpression("^(Corrente|Poupanca)$")]
        [Display(Name = "Tipo da Conta")]
        public string TipoConta { get; set; } = "Corrente";

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Saldo atual")]
        public decimal Saldo { get; set; }
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
