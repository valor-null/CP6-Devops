using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Clientes;

public class IndexModel : PageModel
{
    private readonly IClienteRepository _repo;

    public List<ClienteVm> Clientes { get; private set; } = new();

    public IndexModel(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(); 
        Clientes = entities.Select(c => new ClienteVm
        {
            IdCliente    = c.IdCliente,
            Nome         = c.Nome,
            CPF          = c.CPF,
            Email        = c.Email,
            DataCadastro = c.DataCadastro
        }).ToList();
    }

    public record ClienteVm
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}