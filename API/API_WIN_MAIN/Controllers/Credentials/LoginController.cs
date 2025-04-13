using API_WIN_MAIN.DTOs.PropiedadesDTOs;
using API_WIN_MAIN.Models;
using API_WIN_MAIN.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.Models.Credentials;

namespace API_WIN_MAIN.Controllers.Credentials
{
    [Route("api/Auth/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AplicationDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginUser dto)
        {
            var passwordHasher = new PasswordHasher<Usuario>();

            // Buscar el usuario por email
            var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == dto.email);

            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            // Verificar contraseña
            var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.Contraseña, dto.contraseña);

            if (resultado == PasswordVerificationResult.Success)
            {
                var usuarioSinPassword = new
                {
                    usuario.id_Usuario,
                    usuario.Email,
                    usuario.Nombre,
                    usuario.id_Rol,
                    Rol = new
                    {
                        usuario.Rol.id_Rol,
                        usuario.Rol.Nombre
                    }
                };
                return Ok(usuarioSinPassword);
            }
            else
            {
                return Unauthorized("Credenciales incorrectas");
            }
        }

    }
}

