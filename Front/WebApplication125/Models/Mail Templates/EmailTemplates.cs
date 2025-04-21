using System;
using System.IO;
using System.Text;
using API_WIN_MAIN.Models;

namespace WEB.Util
{
    public static class EmailTemplates
    {
        public static string GenerarResetPasswordHtml(Usuario usuario, string baseApiUrl, String Token)
        {

             string urlReset = $"{baseApiUrl}auth/PasswordRecovery?Token={Token}";

            // Ruta física de la imagen
            string rutaImagen = Path.Combine("wwwroot", "Images", "Brand.png");

            // Leer y convertir a Base64
            string base64Imagen = Convert.ToBase64String(File.ReadAllBytes(rutaImagen));
            string imagenBase64Src = $"data:image/png;base64,{base64Imagen}";

            return $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f5f5f5;
                            padding: 20px;
                            color: #333;
                        }}
                        .container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            max-width: 600px;
                            margin: auto;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                        }}
                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}
                        .btn {{
                            display: inline-block;
                            padding: 12px 24px;
                            background-color: #007bff;
                            color: white;
                            border-radius: 5px;
                            text-decoration: none;
                            margin-top: 20px;
                        }}
                        .footer {{
                            margin-top: 40px;
                            font-size: 0.9em;
                            color: #777;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <img src=""data:image/png;base64,239djg348ws"" alt=""img"" />
                        </div>
                        <h2>Solicitud de restablecimiento de contraseña</h2>
                        <p>Hola <strong>{usuario.Nombre}</strong>,</p>
                        <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta asociada al correo <strong>{usuario.Email}</strong>.</p>
                        <p>Haz clic en el botón de abajo para continuar con el proceso:</p>
                        <p><a class='btn' href='{urlReset}'>Restablecer Contraseña</a></p>
                        <p>Si tú no solicitaste este cambio, puedes ignorar este correo</p>
                        <div class='footer'>
                            &copy; {DateTime.Now.Year} HomeSpace. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}
