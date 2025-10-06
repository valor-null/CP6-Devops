using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Transacoes
{
    public class TransferirModel : PageModel
    {
        private readonly ITransacaoRepository _repo;

        public TransferirModel(ITransacaoRepository repo)
        {
            _repo = repo;
        }

        [BindProperty(SupportsGet = true)]
        public int IdContaOrigem { get; set; }

        [BindProperty, Range(1, int.MaxValue)]
        public int IdContaDestino { get; set; }

        [BindProperty, Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
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

            try
            {
                await _repo.TransferirAsync(IdContaOrigem, IdContaDestino, Valor);
                Success = "Transferência realizada com sucesso.";
                return RedirectToPage("/Transacoes/Index", new { IdConta = IdContaOrigem });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}