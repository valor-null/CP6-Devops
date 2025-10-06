using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;

namespace DimDim.Web.Pages.Clientes;

public class EditModel : PageModel
{
    private readonly IClienteRepository _repo;

    public EditModel(IClienteRepository repo)
    {
        _repo = repo;
    }

    [TempData]
    public string? Success { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public ClienteForm Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Clientes/Index");
        var cli = await _repo.GetByIdAsync(Id);
        if (cli is null) return RedirectToPage("/Clientes/Index");

        Form.Nome = cli.Nome;
        Form.Email = cli.Email;
        Form.CPF = cli.CPF;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Clientes/Index");
        if (!ModelState.IsValid) return Page();

        var cli = await _repo.GetByIdAsync(Id);
        if (cli is null) return RedirectToPage("/Clientes/Index");

        cli.Nome = Form.Nome;
        cli.Email = Form.Email;
        cli.CPF = Form.CPF;

        await _repo.UpdateAsync(cli);
        Success = "Cliente atualizado com sucesso.";
        return RedirectToPage("/Clientes/Index");
    }

    public class ClienteForm
    {
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(14)]
        public string CPF { get; set; } = string.Empty;
    }
}