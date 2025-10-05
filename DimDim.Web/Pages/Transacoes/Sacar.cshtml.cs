using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes
{
    public class SacarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SacarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int IdConta { get; set; }

        [BindProperty]
        public decimal Valor { get; set; }

        [TempData]
        public string? Success { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue) IdConta = id.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (IdConta <= 0) ModelState.AddModelError(nameof(IdConta), "Informe um ID de conta válido.");
            if (Valor <= 0m) ModelState.AddModelError(nameof(Valor), "O valor deve ser maior que zero.");
            if (!ModelState.IsValid) return Page();

            var http = _httpClientFactory.CreateClient("Api");
            var payload = new { IdConta, Valor };
            var paths = new[]
            {
                "api/Transacoes/sacar",
                "api/transacoes/sacar",
                "api/Transacoes/saque",
                "api/transacoes/saque"
            };

            foreach (var p in paths)
            {
                HttpResponseMessage resp;
                try
                {
                    resp = await http.PostAsJsonAsync(p, payload);
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Não foi possível conectar à API.");
                    return Page();
                }

                if (resp.IsSuccessStatusCode)
                {
                    Success = "Saque realizado com sucesso.";
                    return RedirectToPage("/Transacoes/Index", new { IdConta });
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

            ModelState.AddModelError(string.Empty, "Endpoint de saque não encontrado.");
            return Page();
        }
    }
}
