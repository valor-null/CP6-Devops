using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Contas;

public class EditModel : PageModel
{
    private readonly IContaCorrenteRepository _repo;

    public EditModel(IContaCorrenteRepository repo)
    {
        _repo = repo;
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

        var vm = await _repo.GetByIdAsync(Id, ct);
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

        await _repo.UpdateTipoAsync(Id, Form.TipoConta, ct);
        Success = "Conta atualizada com sucesso.";
        return RedirectToPage("/Contas/Index");
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
