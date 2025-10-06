using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Transacoes
{
    public class SacarModel : PageModel
    {
        private readonly ITransacaoRepository _repo;

        public SacarModel(ITransacaoRepository repo)
        {
            _repo = repo;
        }

        [BindProperty(SupportsGet = true)]
        public int IdConta { get; set; }

        [BindProperty, Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
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

            try
            {
                await _repo.SacarAsync(IdConta, Valor);
                Success = "Saque realizado com sucesso.";
                return RedirectToPage("/Transacoes/Index", new { IdConta });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}