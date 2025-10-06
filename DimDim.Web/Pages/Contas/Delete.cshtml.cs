using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Contas;

public class DeleteModel : PageModel
{
    private readonly IContaCorrenteRepository _repo;

    public DeleteModel(IContaCorrenteRepository repo)
    {
        _repo = repo;
    }

    [TempData]
    public string? Success { get; set; }

    [TempData]
    public string? Error { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public ContaVm? Conta { get; private set; }

    [BindProperty]
    public bool Confirm { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Contas/Index");

        var vm = await _repo.GetByIdAsync(Id, ct);
        if (vm == null) return RedirectToPage("/Contas/Index");

        Conta = new ContaVm
        {
            IdConta = vm.IdConta,
            NumeroConta = vm.NumeroConta,
            Saldo = vm.Saldo,
            TipoConta = vm.TipoConta,
            IdCliente = vm.IdCliente
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0 || !Confirm) return RedirectToPage("/Contas/Index");

        await _repo.DeleteAsync(Id, ct);
        Success = "Conta excluída com sucesso.";
        return RedirectToPage("/Contas/Index");
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