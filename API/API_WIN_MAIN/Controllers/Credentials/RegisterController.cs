using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using WEB.Models.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using API_WIN_MAIN.Util;
using System.Text.Json;
namespace API_WIN_MAIN.Controllers.Credentials
{
    [Route("api/Auth/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(AplicationDbContext context, ILogger<RegisterController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] NewUser dto)
        {
            var passwordHasher = new PasswordHasher<Usuario>();
            try
            {
                Usuario usuario = new Usuario()
                {
                    Email = dto.email,
                    Nombre = dto.nombre,
                    id_Rol = (int)RolEnum.Usuario
                };
                usuario.Contraseña = passwordHasher.HashPassword(usuario, dto.contraseña);
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Usuario creado: {JsonSerializer.Serialize(usuario)}");
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear propiedad");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}

