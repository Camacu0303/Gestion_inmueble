using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace WEB.Util
{
    public class SmtpClient
    {
        private const string SenderEmail = "coder.josue@gmail.com";
        private const string SenderPassword = "sfulasmyxukurbul";
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const bool EnableSsl = true;
        private const bool UseDefaultCredentials = false;


        public void EnviarCorreo(string destinatario, string asunto, string cuerpo, bool esHtml = true)
        {
            EnviarCorreo(new List<string> { destinatario }, asunto, cuerpo, esHtml);
        }
        public void EnviarCorreo(List<string> destinatarios, string asunto, string cuerpo, bool esHtml = true)
        {
            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(SenderEmail);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpo;
                mensaje.IsBodyHtml = esHtml;

                foreach (var correo in destinatarios)
                {
                    if (EsCorreoValido(correo))
                    {
                        mensaje.To.Add(correo);
                    }
                }

                if (mensaje.To.Count == 0)
                {
                    throw new InvalidOperationException("No hay destinatarios válidos para enviar el correo.");
                }

                using (var smtp = new System.Net.Mail.SmtpClient(SmtpHost, SmtpPort))
                {
                    smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                    smtp.EnableSsl = EnableSsl;
                    smtp.UseDefaultCredentials = UseDefaultCredentials;

                    try
                    {
                        smtp.Send(mensaje);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Error al enviar el correo: " + ex.Message, ex);
                    }
                }
            }
        }
        private bool EsCorreoValido(string correo)
        {
            try
            {
                var mailAddress = new MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
