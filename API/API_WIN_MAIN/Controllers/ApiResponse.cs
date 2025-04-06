namespace API_WIN_MAIN.Controllers
{
    public class ApiResponse
    {
        public bool success { get; set; }
        public bool Success { get; internal set; }
        public string message { get; set; }
        public string error { get; set; }
        public string data { get; set; }

    }
}
