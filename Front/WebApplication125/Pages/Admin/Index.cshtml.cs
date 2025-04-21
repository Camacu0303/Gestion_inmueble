using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WEB.Util;

namespace WEB.Pages.Admin
{
    public class MantenimientoModel : PageModel
    {
        private readonly IConfiguration _config;

        public string ApiBaseUrl { get; private set; }

        public MantenimientoModel(IConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
            ApiBaseUrl = _config["LocalUrl"];
        }
    }
        
}
