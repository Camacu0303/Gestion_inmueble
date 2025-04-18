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
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<Usuario>
    {
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

