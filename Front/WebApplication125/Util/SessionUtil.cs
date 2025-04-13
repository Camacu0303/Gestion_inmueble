using System.Text.Json;
using static WebApplication125.Pages.Auth.LoginModel;

namespace WEB.Util
{
    public static class SessionUtil
    {
        private const string UserSessionKey = "UserSession";

        public static void SetUser(HttpContext context, CredencialUsuario usuario)
        {
            var jsonUser = JsonSerializer.Serialize(usuario);
            context.Session.SetString(UserSessionKey, jsonUser);
        }

        public static CredencialUsuario? GetUser(HttpContext context)
        {
            var jsonUser = context.Session.GetString(UserSessionKey);
            if (string.IsNullOrEmpty(jsonUser)) return null;

            return JsonSerializer.Deserialize<CredencialUsuario>(jsonUser);
        }

        public static void ClearUser(HttpContext context)
        {
            context.Session.Remove(UserSessionKey);
        }
    }
}
