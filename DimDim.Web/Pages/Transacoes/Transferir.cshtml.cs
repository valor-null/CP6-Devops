using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes
{
    public class TransferirModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TransferirModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int IdContaOrigem { get; set; }

        [BindProperty]
        public int IdContaDestino { get; set; }

        [BindProperty]
        public decimal Valor { get; set; }

        [TempData]
        public string? Success { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue) IdContaOrigem = id.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (IdContaOrigem <= 0) ModelState.AddModelError(nameof(IdContaOrigem), "Informe a conta de origem.");
            if (IdContaDestino <= 0) ModelState.AddModelError(nameof(IdContaDestino), "Informe a conta de destino.");
            if (IdContaDestino == IdContaOrigem) ModelState.AddModelError(nameof(IdContaDestino), "Destino deve ser diferente da origem.");
            if (Valor <= 0m) ModelState.AddModelError(nameof(Valor), "O valor deve ser maior que zero.");
            if (!ModelState.IsValid) return Page();

            var http = _httpClientFactory.CreateClient("Api");

            var payload1 = new { IdContaOrigem, IdContaDestino, Valor };
            var payload2 = new { IdContaDebito = IdContaOrigem, IdContaCredito = IdContaDestino, Valor };

            var paths = new[]
            {
                "api/Transacoes/transferir",
                "api/transacoes/transferir",
                "api/Transacoes/transferencia",
                "api/transacoes/transferencia"
            };

            foreach (var p in paths)
            {
                HttpResponseMessage resp;
                try
                {
                    resp = await http.PostAsJsonAsync(p, payload1);
                    if (resp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        resp = await http.PostAsJsonAsync(p, payload2);
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Não foi possível conectar à API.");
                    return Page();
                }

                if (resp.IsSuccessStatusCode)
                {
                    Success = "Transferência realizada com sucesso.";
                    return RedirectToPage("/Transacoes/Index", new { IdConta = IdContaOrigem });
                }

                if (resp.StatusCode != HttpStatusCode.NotFound)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    try
                    {
                        var doc = JsonDocument.Parse(body);
                        if (doc.RootElement.TryGetProperty("errors", out var errors))
                        {
                            foreach (var prop in errors.EnumerateObject())
                            {
                                foreach (var msg in prop.Value.EnumerateArray())
                                    ModelState.AddModelError(prop.Name, msg.GetString() ?? "Erro");
                            }
                        }
                        else
                        {
                            var detail = doc.RootElement.TryGetProperty("detail", out var d) ? d.GetString() : null;
                            var title = doc.RootElement.TryGetProperty("title", out var t) ? t.GetString() : null;
                            ModelState.AddModelError(string.Empty, detail ?? title ?? body);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, body);
                    }
                    return Page();
                }
            }

            ModelState.AddModelError(string.Empty, "Endpoint de transferência não encontrado.");
            return Page();
        }
    }
}
