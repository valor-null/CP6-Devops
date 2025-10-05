using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly DbContext _db;

        public TransacoesController(DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransacaoDto>>> Get([FromQuery] int IdConta, [FromQuery] int Take = 10, [FromQuery] int Skip = 0)
        {
            if (IdConta <= 0) return BadRequest();

            var itens = await _db.Set<Transacao>().AsNoTracking()
                .Where(t => t.IdConta == IdConta)
                .OrderByDescending(t => t.DataHora)
                .Skip(Skip)
                .Take(Take)
                .Select(t => new TransacaoDto
                {
                    DataHora = t.DataHora,
                    Tipo = t.Tipo,
                    Valor = t.Valor
                })
                .ToListAsync();

            return itens;
        }

        [HttpGet("conta/{id}")]
        public async Task<ActionResult<List<TransacaoDto>>> GetByConta([FromRoute] int id, [FromQuery] int take = 10, [FromQuery] int skip = 0)
        {
            if (id <= 0) return BadRequest();

            var itens = await _db.Set<Transacao>().AsNoTracking()
                .Where(t => t.IdConta == id)
                .OrderByDescending(t => t.DataHora)
                .Skip(skip)
                .Take(take)
                .Select(t => new TransacaoDto
                {
                    DataHora = t.DataHora,
                    Tipo = t.Tipo,
                    Valor = t.Valor
                })
                .ToListAsync();

            return itens;
        }
    }

    public class TransacaoDto
    {
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }

    public class Transacao
    {
        public int Id { get; set; }
        public int IdConta { get; set; }
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
