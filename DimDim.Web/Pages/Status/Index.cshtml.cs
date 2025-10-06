using Microsoft.AspNetCore.Mvc.RazorPages;
using DimDim.Infrastructure.Data;

namespace DimDim.Web.Pages.Status;

public class IndexModel : PageModel
{
    private readonly DimDimContext _ctx;

    public string ApiStatus { get; private set; } = "down";
    public string DbStatus { get; private set; } = "down";
    public DateTime? Time { get; private set; }

    public IndexModel(DimDimContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        ApiStatus = "up";
        Time = DateTime.Now;

        try
        {
            var ok = await _ctx.Database.CanConnectAsync(ct);
            DbStatus = ok ? "up" : "down";
        }
        catch
        {
            DbStatus = "down";
        }
    }
}