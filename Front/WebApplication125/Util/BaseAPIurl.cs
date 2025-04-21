namespace WEB.Util
{
    public class BaseAPIurl
    {
        private readonly IHttpClientFactory _httpClient;
        public BaseAPIurl(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            var client = _httpClient.CreateClient("ApiClient");
            BaseUrl = client.ToString();
        }
        public string BaseUrl { get; set; }
    }
}
