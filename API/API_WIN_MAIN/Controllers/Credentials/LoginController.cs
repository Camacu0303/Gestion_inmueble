using API_WIN_MAIN.DTOs.Credentials;
using API_WIN_MAIN.DTOs.PropiedadesDTOs;
using API_WIN_MAIN.Models;
using API_WIN_MAIN.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEB.Models.Credentials;
using static API_WIN_MAIN.Controllers.Credentials.LoginController;

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

        [HttpPost("reset-password")]
        public IActionResult ValidarToken([FromBody] string token)
        {
            var principal = TokenHelper.ValidarJwtToken(token);
            if (principal == null) return Unauthorized("Token inválido o expirado");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var nombre = _context.Usuarios.FirstOrDefault(u => u.Email == email)?.Nombre;
            return Ok(new { nombre, email });
        }


        [HttpPost("update-password")]
        public async Task<IActionResult> ActualizarPasswordAsync([FromBody] ResetUser model)
        {
            var principal = TokenHelper.ValidarJwtToken(model.Token);
            if (principal == null) return Unauthorized("Token inválido o expirado");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.Contraseña = passwordHasher.HashPassword(usuario, model.contraseña);
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return Ok("Contraseña actualizada.");
        }


        [HttpPost("quickHash")]
        public IActionResult Hash([FromQuery] String email)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) return NotFound("Usuario no encontrado");

            var token = TokenHelper.GenerarJwtToken(usuario.id_Usuario, usuario.Email);
            return Ok(token);
        }
        public class ResetPasswordDto
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NuevaPassword { get; set; }
        }

    }
}

