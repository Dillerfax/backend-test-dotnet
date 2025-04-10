using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FCamara.Test.Estacionamento.Api.Data;
using FCamara.Test.Estacionamento.Api.Entities;
using FCamara.Test.Estacionamento.Api.DTOs;

namespace FCamara.Test.Estacionamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabelecimentosController : ControllerBase
    {
        private readonly EstacionamentoDbContext _context;

        public EstabelecimentosController(EstacionamentoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estabelecimento>>> GetEstabelecimentos()
        {
            var estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return Ok(estabelecimentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estabelecimento>> GetEstabelecimento(int id)
        {
            var estabelecimento = await _context.Estabelecimentos.FindAsync(id);

            if (estabelecimento == null)
            {
                return NotFound();
            }

            return Ok(estabelecimento);
        }

        [HttpPost]
        public async Task<ActionResult<Estabelecimento>> PostEstabelecimento(Estabelecimento estabelecimento)
        {
            _context.Estabelecimentos.Add(estabelecimento);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstabelecimento), new { id = estabelecimento.Id }, estabelecimento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstabelecimento(int id, Estabelecimento estabelecimento)
        {
            if (id != estabelecimento.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID do objeto.");
            }

            _context.Entry(estabelecimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstabelecimentoExists(id))
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
        public async Task<IActionResult> DeleteEstabelecimento(int id)
        {
            var estabelecimento = await _context.Estabelecimentos.FindAsync(id);
            if (estabelecimento == null)
            {
                return NotFound();
            }

            _context.Estabelecimentos.Remove(estabelecimento);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstabelecimentoExists(int id)
        {
            return _context.Estabelecimentos.Any(e => e.Id == id);
        }

        [HttpPost("{idEstabelecimento}/entrada")]
        public async Task<ActionResult<RegistroEstacionamento>> RegistrarEntrada(int idEstabelecimento, [FromBody] EntradaSaidaRequestDto request)
        {
            var estabelecimento = await _context.Estabelecimentos.FindAsync(idEstabelecimento);
            if (estabelecimento == null)
            {
                return NotFound(new { message = $"Estabelecimento com ID {idEstabelecimento} não encontrado." });
            }

            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.Placa == request.Placa);
            if (veiculo == null)
            {
                return NotFound(new { message = $"Veículo com placa {request.Placa} não cadastrado." });
            }

            bool veiculoJaEstacionado = await _context.RegistrosEstacionamento
                                                .AnyAsync(r => r.VeiculoId == veiculo.Id && r.HoraSaida == null);
            if (veiculoJaEstacionado)
            {
                return Conflict(new { message = $"Veículo com placa {request.Placa} já se encontra estacionado." });
            }

            var vagasOcupadas = _context.RegistrosEstacionamento
                                        .Where(r => r.EstabelecimentoId == idEstabelecimento && r.HoraSaida == null)
                                        .Include(r => r.Veiculo)
                                        .AsEnumerable() 
                                        .GroupBy(r => r.Veiculo.Tipo)
                                        .ToDictionary(g => g.Key, g => g.Count());

            int vagasOcupadasCarro = vagasOcupadas.GetValueOrDefault(TipoVeiculo.Carro, 0);
            int vagasOcupadasMoto = vagasOcupadas.GetValueOrDefault(TipoVeiculo.Moto, 0);

            if (veiculo.Tipo == TipoVeiculo.Carro && vagasOcupadasCarro >= estabelecimento.QuantidadeVagasCarros)
            {
                return Conflict(new { message = "Não há vagas disponíveis para carros neste estabelecimento." });
            }
            if (veiculo.Tipo == TipoVeiculo.Moto && vagasOcupadasMoto >= estabelecimento.QuantidadeVagasMotos)
            {
                return Conflict(new { message = "Não há vagas disponíveis para motos neste estabelecimento." });
            }

            var novoRegistro = new RegistroEstacionamento
            {
                EstabelecimentoId = idEstabelecimento,
                VeiculoId = veiculo.Id,
                HoraEntrada = DateTime.UtcNow
            };

            _context.RegistrosEstacionamento.Add(novoRegistro);

            await _context.SaveChangesAsync();
            await _context.Entry(novoRegistro).Reference(r => r.Veiculo).LoadAsync();
            await _context.Entry(novoRegistro).Reference(r => r.Estabelecimento).LoadAsync();

            return CreatedAtAction(nameof(GetRegistroEstacionamento),
                                   new { idEstabelecimento = idEstabelecimento, idRegistro = novoRegistro.Id },
                                   novoRegistro);
        }


        [HttpPost("{idEstabelecimento}/saida")]
        public async Task<ActionResult<RegistroEstacionamento>> RegistrarSaida(int idEstabelecimento, [FromBody] EntradaSaidaRequestDto request)
        {
            var estabelecimento = await _context.Estabelecimentos.FindAsync(idEstabelecimento);
            if (estabelecimento == null)
            {
                return NotFound(new { message = $"Estabelecimento com ID {idEstabelecimento} não encontrado." });
            }

            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.Placa == request.Placa);
            if (veiculo == null)
            {
                return NotFound(new { message = $"Veículo com placa {request.Placa} não cadastrado." });
            }

            var registroAtivo = await _context.RegistrosEstacionamento
                                            .FirstOrDefaultAsync(r => r.EstabelecimentoId == idEstabelecimento &&
                                                                    r.VeiculoId == veiculo.Id &&
                                                                    r.HoraSaida == null);

            if (registroAtivo == null)
            {
                return NotFound(new { message = $"Não há registro de entrada ativo para o veículo {request.Placa} neste estabelecimento." });
            }

            registroAtivo.HoraSaida = DateTime.UtcNow;

            _context.RegistrosEstacionamento.Update(registroAtivo);

            await _context.SaveChangesAsync();
            await _context.Entry(registroAtivo).Reference(r => r.Veiculo).LoadAsync();
            await _context.Entry(registroAtivo).Reference(r => r.Estabelecimento).LoadAsync();

            return Ok(registroAtivo);
        }

        [HttpGet("{idEstabelecimento}/registros/{idRegistro}")]
        public async Task<ActionResult<RegistroEstacionamento>> GetRegistroEstacionamento(int idEstabelecimento, int idRegistro)
        {
            var registro = await _context.RegistrosEstacionamento
                                        .Include(r => r.Veiculo)
                                        .Include(r => r.Estabelecimento)
                                        .FirstOrDefaultAsync(r => r.Id == idRegistro && r.EstabelecimentoId == idEstabelecimento);

            if (registro == null)
            {
                return NotFound();
            }

            return Ok(registro);
        }

        [HttpGet("{idEstabelecimento}/registros")]
        public async Task<ActionResult<IEnumerable<RegistroEstacionamento>>> GetRegistrosPorEstabelecimento(int idEstabelecimento)
        {
            bool estabelecimentoExiste = await _context.Estabelecimentos.AnyAsync(e => e.Id == idEstabelecimento);
            if (!estabelecimentoExiste)
            {
                return NotFound(new { message = $"Estabelecimento com ID {idEstabelecimento} não encontrado." });
            }

            var registros = await _context.RegistrosEstacionamento
                                        .Where(r => r.EstabelecimentoId == idEstabelecimento)
                                        .Include(r => r.Veiculo)
                                        .Include(r => r.Estabelecimento)
                                        .OrderByDescending(r => r.HoraEntrada)                          
                                        .ToListAsync();

            return Ok(registros);
        }
    }
}
