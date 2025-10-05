using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes;

public class DepositarModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public DepositarModel(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }

    [BindProperty]
    public DepositoForm Form { get; set; } = new();

    public void OnGet(int? idConta)
    {
        if (idConta is > 0) Form.IdConta = idConta.Value;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("Api");
        var res = await client.PostAsJsonAsync("/api/transacoes/deposito", new
        {
            idConta = Form.IdConta,
            valor = Form.Valor
        }, ct);

        if (res.IsSuccessStatusCode)
            return RedirectToPage("/Transacoes/Index", new { idConta = Form.IdConta });

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha no depósito." : msg);
        return Page();
    }

    public class DepositoForm
    {
        [Range(1, int.MaxValue)]
        public int IdConta { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Valor { get; set; }
    }
}