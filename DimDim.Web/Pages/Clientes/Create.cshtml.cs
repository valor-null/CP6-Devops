using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Core.Interfaces;
using DimDim.Core.Entities;

namespace DimDim.Web.Pages.Clientes;

public class CreateModel : PageModel
{
    private readonly IClienteRepository _repo;

    public CreateModel(IClienteRepository repo)
    {
        _repo = repo;
    }

    [TempData]
    public string? Success { get; set; }

    [BindProperty]
    public ClienteForm Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        var cli = new Cliente
        {
            Nome = Form.Nome,
            CPF = Form.CPF,
            Email = Form.Email
        };

        await _repo.CreateAsync(cli, ct);
        Success = "Cliente criado com sucesso.";
        return RedirectToPage("/Clientes/Index");
    }

    public class ClienteForm
    {
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{11}$")]
        public string CPF { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;
    }
}