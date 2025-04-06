using API_WIN_MAIN.DTOs.ContratosDTOs;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_WIN_MAIN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratosController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly ILogger<ContratosController> _logger;

        public ContratosController(AplicationDbContext context, ILogger<ContratosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContratoDto>>> Get()
        {
            return await _context.Contratos
                .Include(c => c.Propiedad)
                .Include(c => c.Cliente)
                .Include(c => c.EstadoContrato)
                .Select(c => new ContratoDto
                {
                    Id = c.id_Contrato,
                    PropiedadNombre = c.Propiedad.Nombre,
                    ClienteNombre = c.Cliente.Nombre,
                    Fecha = c.Fecha,
                    Monto = c.Monto,
                    Duracion = c.Duracion,
                    Estado = c.EstadoContrato.Nombre
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContratoDetailDto>> GetById(int id)
        {
            var contrato = await _context.Contratos
                .Include(c => c.Propiedad)
                .Include(c => c.Cliente)
                .Include(c => c.EstadoContrato)
                .FirstOrDefaultAsync(c => c.id_Contrato == id);

            if (contrato == null)
            {
                return NotFound();
            }

            return new ContratoDetailDto
            {
                Id = contrato.id_Contrato,
                IdPropiedad = contrato.id_Propiedad,
                IdCliente = contrato.id_Cliente,
                Fecha = contrato.Fecha,
                Monto = contrato.Monto,
                Duracion = contrato.Duracion,
                IdEstado = contrato.id_Estado,
                PropiedadNombre = contrato.Propiedad?.Nombre,
                ClienteNombre = contrato.Cliente?.Nombre,
                EstadoNombre = contrato.EstadoContrato?.Nombre
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContratoCreateDto dto)
        {
            try
            {
                _logger.LogInformation($"Recibido DTO: {JsonSerializer.Serialize(dto)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var contrato = new Contrato
                {
                    id_Propiedad = dto.IdPropiedad,
                    id_Cliente = dto.IdCliente,
                    Fecha = dto.Fecha,
                    Monto = dto.Monto,
                    Duracion = dto.Duracion,
                    id_Estado = dto.IdEstado,
                };

                _context.Contratos.Add(contrato);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Contrato guardado con ID: {contrato.id_Contrato}");

                return Ok(new { id = contrato.id_Contrato });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos");
                return StatusCode(500, new { error = "Error al guardar en base de datos", details = dbEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ContratoUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            contrato.id_Propiedad = dto.IdPropiedad;
            contrato.id_Cliente = dto.IdCliente;
            contrato.Fecha = dto.Fecha;
            contrato.Monto = dto.Monto;
            contrato.Duracion = dto.Duracion;
            contrato.id_Estado = dto.IdEstado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratoExists(id))
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
        public async Task<IActionResult> Delete(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.id_Contrato == id);
        }
    }
}
