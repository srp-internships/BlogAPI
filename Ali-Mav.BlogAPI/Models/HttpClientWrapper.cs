namespace Ali_Mav.BlogAPI.Models
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _httpClient.GetAsync(url);
        }
    }
}
