using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Transacoes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int IdConta { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Take { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int Skip { get; set; } = 0;

        public List<TransacaoVm> Transacoes { get; set; } = new();

        public string? Error { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (IdConta <= 0)
            {
                Transacoes = new List<TransacaoVm>();
                return Page();
            }

            var http = _httpClientFactory.CreateClient("Api");
            var url = $"api/Transacoes/conta/{IdConta}?take={Take}&skip={Skip}";

            HttpResponseMessage resp;
            try
            {
                resp = await http.GetAsync(url);
            }
            catch
            {
                Error = "Não foi possível conectar à API.";
                return Page();
            }

            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                Error = $"Erro ao carregar transações ({(int)resp.StatusCode}). {body}";
                Transacoes = new List<TransacaoVm>();
                return Page();
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var list = await resp.Content.ReadFromJsonAsync<List<TransacaoVm>>(options);
            Transacoes = list ?? new List<TransacaoVm>();
            return Page();
        }

        public class TransacaoVm
        {
            public DateTime DataHora { get; set; }
            public string Tipo { get; set; } = "";
            public decimal Valor { get; set; }
        }
    }
}
