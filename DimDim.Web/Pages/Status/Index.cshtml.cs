using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Status;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _json;

    public string ApiStatus { get; private set; } = "down";
    public string DbStatus { get; private set; } = "down";
    public DateTime? Time { get; private set; }

    public IndexModel(IHttpClientFactory httpFactory, JsonSerializerOptions json)
    {
        _httpFactory = httpFactory;
        _json = json;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("Api");

        try
        {
            using var res = await client.GetAsync("/api/health", ct);
            if (res.IsSuccessStatusCode)
            {
                var doc = await res.Content.ReadFromJsonAsync<Dictionary<string, object>>(_json, ct);
                if (doc != null)
                {
                    if (doc.TryGetValue("status", out var s)) ApiStatus = s?.ToString() ?? "ok";
                    if (doc.TryGetValue("time", out var t) && DateTime.TryParse(t?.ToString(), out var dt)) Time = dt;
                }
                else
                {
                    ApiStatus = "ok";
                }
            }
        }
        catch { }

        try
        {
            using var res2 = await client.GetAsync("/api/health/db", ct);
            if (res2.IsSuccessStatusCode)
            {
                var doc = await res2.Content.ReadFromJsonAsync<Dictionary<string, string>>(_json, ct);
                if (doc != null && doc.TryGetValue("database", out var s)) DbStatus = s ?? "up";
                else DbStatus = "up";
            }
        }
        catch { }
    }
}