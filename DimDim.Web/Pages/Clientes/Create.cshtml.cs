using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public CreateModel(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }

    [BindProperty]
    public ClienteForm Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("Api");
        var res = await client.PostAsJsonAsync("/api/clientes", new
        {
            nome = Form.Nome,
            cpf = Form.CPF,
            email = Form.Email
        }, ct);

        if (res.IsSuccessStatusCode) return RedirectToPage("/Clientes/Index");

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha ao criar cliente." : msg);
        return Page();
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