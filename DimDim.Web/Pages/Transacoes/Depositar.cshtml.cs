using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Transacoes;

public class DepositarModel : PageModel
{
    private readonly ITransacaoRepository _repo;

    public DepositarModel(ITransacaoRepository repo)
    {
        _repo = repo;
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

        try
        {
            await _repo.DepositarAsync(Form.IdConta, Form.Valor, ct);
            return RedirectToPage("/Transacoes/Index", new { idConta = Form.IdConta });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public class DepositoForm
    {
        [Range(1, int.MaxValue)]
        public int IdConta { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Valor { get; set; }
    }
}