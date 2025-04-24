using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WIN_MAIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : BaseController<Rol>
    {
        public RolController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PropiedadesController : BaseController<Propiedad>
    {
        public PropiedadesController(AplicationDbContext context) : base(context)
        {
        }
        [HttpGet("filtros")]
        public async Task<IActionResult> FiltrarPropiedades(
            string? nombre,
            decimal? precioMin,
            decimal? precioMax,
            int? id_tipo,
            int? id_estado,
            string? direccion
         
        )
        {
            if (precioMin.HasValue && precioMax.HasValue && precioMin > precioMax)
            {
                return BadRequest("El precio mínimo no puede ser mayor que el precio máximo.");
            }

            var query = GetContext().Set <Propiedad>().AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.nombre.ToLower().Contains(nombre.ToLower()));

            if (!string.IsNullOrEmpty(direccion))
                query = query.Where(p => p.direccion.ToLower().Contains(direccion.ToLower()));

            if (precioMin.HasValue)
                query = query.Where(p => p.precio >= precioMin.Value);

            if (precioMax.HasValue)
                query = query.Where(p => p.precio <= precioMax.Value);

            if (id_tipo.HasValue)
                query = query.Where(p => p.id_tipo == id_tipo.Value);

            if (id_estado.HasValue)
                query = query.Where(p => p.id_estado == id_estado.Value);

            var resultado = await query
                .Include(p => p.TipoPropiedad)
                .Include(p => p.EstadoPropiedad)
                .Include(p => p.Usuario)
            
                .ToListAsync();

            return Ok(resultado);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<Usuario>
    {
        // Constructor que recibe el contexto y prepara el DbSet para uso posterior
        public UsuarioController(AplicationDbContext context) : base(context)
        {
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class TipoPropiedadController : BaseController<TipoPropiedad>
    {
        public TipoPropiedadController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EstadoPropiedadController : BaseController<EstadoPropiedad>
    {
        public EstadoPropiedadController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : BaseController<Cliente>
    {
        public ClienteController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AgenteController : BaseController<Agente>
    {
        public AgenteController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EstadoContratoController : BaseController<EstadoContrato>
    {
        public EstadoContratoController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : BaseController<Contrato>
    {
        public ContratoController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EstadoPagoController : BaseController<EstadoPago>
    {
        public EstadoPagoController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : BaseController<Pago>
    {
        public PagoController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class VisitaController : BaseController<Visita>
    {
        public VisitaController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionController : BaseController<Calificacion>
    {
        public CalificacionController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : BaseController<Comentario>
    {
        public ComentarioController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EstadoNotificacionController : BaseController<EstadoNotificacion>
    {
        public EstadoNotificacionController(AplicationDbContext context) : base(context)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : BaseController<Notificacion>
    {
        public NotificacionController(AplicationDbContext context) : base(context)
        {
        }
    }
}

