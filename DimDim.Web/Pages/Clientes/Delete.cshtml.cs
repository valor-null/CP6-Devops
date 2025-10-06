using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Clientes;

public class DeleteModel : PageModel
{
    private readonly IClienteRepository _repo;

    public DeleteModel(IClienteRepository repo) => _repo = repo;

    [TempData] public string? Success { get; set; }
    [TempData] public string? Error { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public ClienteVm? Cliente { get; private set; }

    [BindProperty]
    public bool Confirm { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Clientes/Index");

        var cli = await _repo.GetByIdAsync(Id, ct);
        if (cli is null) return RedirectToPage("/Clientes/Index");

        Cliente = new ClienteVm
        {
            IdCliente = cli.IdCliente,
            Nome = cli.Nome,
            CPF = cli.CPF,
            Email = cli.Email,
            DataCadastro = cli.DataCadastro
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0 || !Confirm) return RedirectToPage("/Clientes/Index");

        try
        {
            await _repo.DeleteAsync(Id, ct);
            Success = "Cliente excluído com sucesso.";
            return RedirectToPage("/Clientes/Index");
        }
        catch
        {
            Error = "Não é possível excluir o cliente: existem contas vinculadas. Exclua as contas primeiro.";
            return await OnGetAsync(ct);
        }
    }

    public class ClienteVm
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}