using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;
using DimDim.Core.Entities;
using System.Linq;

namespace DimDim.Web.Pages.Contas;

public class CreateModel : PageModel
{
    private readonly IContaCorrenteRepository _repoContas;
    private readonly IClienteRepository _repoClientes;

    public CreateModel(IContaCorrenteRepository repoContas, IClienteRepository repoClientes)
    {
        _repoContas = repoContas;
        _repoClientes = repoClientes;
    }

    [TempData]
    public string? Success { get; set; }

    [BindProperty]
    public ContaForm Form { get; set; } = new();

    public List<ClienteItem> Clientes { get; private set; } = new();

    public async Task OnGetAsync(CancellationToken ct)
    {
        var list = await _repoClientes.GetAllAsync(ct);
        Clientes = list.Select(c => new ClienteItem { IdCliente = c.IdCliente, Nome = c.Nome }).ToList();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(ct);
            return Page();
        }

        var conta = new ContaCorrente
        {
            IdCliente = Form.IdCliente,
            NumeroConta = Form.NumeroConta,
            TipoConta = Form.TipoConta,
            Saldo = 0m
        };

        await _repoContas.CreateAsync(conta, ct);
        Success = "Conta criada com sucesso.";
        return RedirectToPage("/Contas/Index");
    }

    public class ContaForm
    {
        [Range(1, int.MaxValue)]
        public int IdCliente { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string NumeroConta { get; set; } = string.Empty;

        [Required, RegularExpression("^(Corrente|Poupanca)$")]
        public string TipoConta { get; set; } = "Corrente";
    }

    public class ClienteItem
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
