using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Transacoes
{
    public class IndexModel : PageModel
    {
        private readonly ITransacaoRepository _repo;

        public IndexModel(ITransacaoRepository repo)
        {
            _repo = repo;
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

            try
            {
                var list = await _repo.ListarPorContaAsync(IdConta, Take, Skip);
                Transacoes = list.Select(t => new TransacaoVm
                {
                    DataHora = t.DataHora,
                    Tipo = t.Tipo,
                    Valor = t.Valor
                }).ToList();
                return Page();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Transacoes = new List<TransacaoVm>();
                return Page();
            }
        }

        public class TransacaoVm
        {
            public DateTime DataHora { get; set; }
            public string Tipo { get; set; } = "";
            public decimal Valor { get; set; }
        }
    }
}