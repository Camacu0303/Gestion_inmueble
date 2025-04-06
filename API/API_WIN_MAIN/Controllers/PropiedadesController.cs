using System.Text.Json;
using API_WIN_MAIN.DTOs.PropiedadesDTOs;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WIN_MAIN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropiedadesController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly ILogger<PropiedadesController> _logger;

        public PropiedadesController(AplicationDbContext context, ILogger<PropiedadesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Propiedad>> CrearPropiedad([FromBody] PropiedadCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var propiedad = new Propiedad
                {
                    Nombre = dto.Nombre,
                    Direccion = dto.Direccion,
                    Precio = dto.Precio,
                    id_Tipo = dto.id_Tipo,
                    id_Estado = dto.id_Estado,
                    id_Usuario = dto.id_Usuario
                };

                _context.Propiedades.Add(propiedad);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Propiedad creada: {JsonSerializer.Serialize(propiedad)}");
                return CreatedAtAction(nameof(GetById), new { id = propiedad.id_Propiedad }, propiedad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear propiedad");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropiedadDto>>> Get(
        [FromQuery] string? filtroNombre = null,
        [FromQuery] decimal? precioMin = null,
        [FromQuery] decimal? precioMax = null,
        [FromQuery] int? idTipo = null,
        [FromQuery] int? idEstado = null)
    {
        try
        {
            _logger.LogInformation("Iniciando consulta de propiedades");
            
            var query = _context.Propiedades
                .Include(p => p.TipoPropiedad)
                .Include(p => p.EstadoPropiedad)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(filtroNombre))
                query = query.Where(p => p.Nombre.Contains(filtroNombre));

            if (precioMin.HasValue)
                query = query.Where(p => p.Precio >= precioMin.Value);

            if (precioMax.HasValue)
                query = query.Where(p => p.Precio <= precioMax.Value);

            if (idTipo.HasValue)
                query = query.Where(p => p.id_Tipo == idTipo.Value);

            if (idEstado.HasValue)
                query = query.Where(p => p.id_Estado == idEstado.Value);

            var propiedades = await query
                .OrderBy(p => p.Nombre)
                .Select(p => new PropiedadDto
                {
                    Id = p.id_Propiedad,
                    Nombre = p.Nombre,
                    Direccion = p.Direccion,
                    Precio = p.Precio,
                    Tipo = p.TipoPropiedad.Nombre,
                    Estado = p.EstadoPropiedad.Nombre,
                })
                .ToListAsync();

            _logger.LogInformation($"Retornadas {propiedades.Count} propiedades");
            return Ok(propiedades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener propiedades");
            return StatusCode(500, "Error interno del servidor");
        }
    }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando eliminación de propiedad ID: {id}");

                var propiedad = await _context.Propiedades
                    .Include(p => p.Contratos)
                    .FirstOrDefaultAsync(p => p.id_Propiedad == id);

                if (propiedad == null)
                {
                    _logger.LogWarning($"Propiedad con ID {id} no encontrada");
                    return NotFound();
                }

                // Validar si tiene contratos asociados
                if (propiedad.Contratos.Any())
                {
                    _logger.LogWarning($"Propiedad ID {id} tiene {propiedad.Contratos.Count} contratos asociados");
                    return BadRequest(new
                    {
                        success = false,
                        message = "No se puede eliminar la propiedad porque tiene contratos asociados",
                        contratos = propiedad.Contratos.Select(c => c.id_Contrato)
                    });
                }

                _context.Propiedades.Remove(propiedad);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Propiedad ID {id} eliminada exitosamente");
                return Ok(new { success = true, message = "Propiedad eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar propiedad ID {id}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropiedadDetailDto>> GetById(int id)
        {
            try
            {
                var propiedad = await _context.Propiedades
                    .Include(p => p.TipoPropiedad)
                    .Include(p => p.EstadoPropiedad)
                    .FirstOrDefaultAsync(p => p.id_Propiedad == id);

                if (propiedad == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "Propiedad no encontrada"
                    });
                }

                return Ok(new PropiedadDetailDto
                {
                    Id = propiedad.id_Propiedad,
                    Nombre = propiedad.Nombre,
                    Direccion = propiedad.Direccion,
                    Precio = propiedad.Precio,
                    IdTipo = propiedad.id_Tipo,
                    IdEstado = propiedad.id_Estado,
                    IdUsuario = propiedad.id_Usuario,
                    Tipo = propiedad.TipoPropiedad?.Nombre,
                    Estado = propiedad.EstadoPropiedad?.Nombre
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener propiedad ID: {id}");
                return StatusCode(500, new ApiResponse
                {
                    success = false,
                    message = "Error interno al obtener propiedad"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PropiedadUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        success = false,
                        message = "IDs no coinciden"
                    });
                }

                var propiedad = await _context.Propiedades.FindAsync(id);
                if (propiedad == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "Propiedad no encontrada"
                    });
                }

                // Validación de negocio
                if (dto.Precio <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        success = false,
                        message = "El precio debe ser mayor a cero"
                    });
                }

                // Mapeo de propiedades
                propiedad.Nombre = dto.Nombre;
                propiedad.Direccion = dto.Direccion;
                propiedad.Precio = dto.Precio;
                propiedad.id_Tipo = dto.IdTipo;
                propiedad.id_Estado = dto.IdEstado;

                _context.Entry(propiedad).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Propiedad actualizada correctamente"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Error de concurrencia al actualizar propiedad ID: {id}");
                return StatusCode(409, new ApiResponse
                {
                    success = false,
                    message = "Error de concurrencia: la propiedad fue modificada por otro usuario"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar propiedad ID: {id}");
                return StatusCode(500, new ApiResponse
                {
                    success = false,
                    message = "Error interno al actualizar propiedad"
                });
            }
        }
    }
}
