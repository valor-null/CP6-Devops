using DimDim.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly DimDimContext _ctx;

    public HealthController(DimDimContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(new { status = "ok", time = DateTime.UtcNow });
    }

    [HttpGet("db")]
    public async Task<IActionResult> Database()
    {
        var canConnect = await _ctx.Database.CanConnectAsync();
        return canConnect ? Ok(new { database = "up" }) : StatusCode(503, new { database = "down" });
    }
}