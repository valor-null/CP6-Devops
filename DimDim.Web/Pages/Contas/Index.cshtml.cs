using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Contas;

public class IndexModel : PageModel
{
    private readonly IContaCorrenteRepository _repo;

    public IndexModel(IContaCorrenteRepository repo)
    {
        _repo = repo;
    }

    public List<ContaVm> Contas { get; private set; } = new();
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            var data = await _repo.GetAllAsync(ct);
            Contas = data.Select(c => new ContaVm
            {
                IdConta = c.IdConta,
                NumeroConta = c.NumeroConta,
                Saldo = c.Saldo,
                TipoConta = c.TipoConta,
                IdCliente = c.IdCliente
            }).ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Falha ao carregar contas: {ex.Message}";
            Contas = new List<ContaVm>();
        }
    }

    public record ContaVm
    {
        public int IdConta { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public int IdCliente { get; set; }
    }
}