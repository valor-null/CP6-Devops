using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public EditModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
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

        var client = _httpFactory.CreateClient("Api");
        var res = await client.GetAsync($"/api/clientes/{Id}", ct);
        if (res.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToPage("/Clientes/Index");
        res.EnsureSuccessStatusCode();

        var vm = await res.Content.ReadFromJsonAsync<ClienteVm>(_json, ct);
        if (vm == null) return RedirectToPage("/Clientes/Index");

        Form.Nome = vm.Nome;
        Form.Email = vm.Email;
        Form.CPF = vm.CPF;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Id <= 0) return RedirectToPage("/Clientes/Index");
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("Api");
        var payload = new { nome = Form.Nome, email = Form.Email };
        var res = await client.PutAsJsonAsync($"/api/clientes/{Id}", payload, _json, ct);

        if (res.IsSuccessStatusCode)
        {
            Success = "Cliente atualizado com sucesso.";
            return RedirectToPage("/Clientes/Index");
        }

        var msg = await res.Content.ReadAsStringAsync(ct);
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(msg) ? "Falha ao atualizar cliente." : msg);
        return Page();
    }

    public class ClienteForm
    {
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;
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
