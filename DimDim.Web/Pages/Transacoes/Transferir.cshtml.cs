using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes;

public class TransferirModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public TransferirModel(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }

    [BindProperty]
    public TransferenciaForm Form { get; set; } = new();

    public void OnGet(int? idContaOrigem, int? idContaDestino)
    {
        if (idContaOrigem is > 0) Form.IdContaOrigem = idContaOrigem.Value;
        if (idContaDestino is > 0) Form.IdContaDestino = idContaDestino.Value;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("Api");
        var res = await client.PostAsJsonAsync("/api/transacoes/transferencia", new
        {
            idContaOrigem = Form.IdContaOrigem,
            idContaDestino = Form.IdContaDestino,
            valor = Form.Valor
        }, ct);

        if (res.IsSuccessStatusCode)
            return RedirectToPage("/Transacoes/Index", new { idConta = Form.IdContaOrigem });

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha na transferência." : msg);
        return Page();
    }

    public class TransferenciaForm
    {
        [Range(1, int.MaxValue)]
        public int IdContaOrigem { get; set; }

        [Range(1, int.MaxValue)]
        public int IdContaDestino { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Valor { get; set; }
    }
}