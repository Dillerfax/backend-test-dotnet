using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FCamara.Test.Estacionamento.Api.Data;
using FCamara.Test.Estacionamento.Api.Entities;

namespace FCamara.Test.Estacionamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly EstacionamentoDbContext _context;

        public VeiculosController(EstacionamentoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            var veiculos = await _context.Veiculos.ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            return Ok(veiculo);
        }

        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            bool placaExiste = await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa);
            if (placaExiste)
            {
                return Conflict(new { message = $"Veículo com a placa {veiculo.Placa} já cadastrado." });
            }

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, veiculo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID do objeto.");
            }

            bool placaDuplicada = await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa && v.Id != id);
            if (placaDuplicada)
            {
                return Conflict(new { message = $"A placa {veiculo.Placa} já está em uso por outro veículo." });
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
