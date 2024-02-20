using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Service.Interface;
using Newtonsoft.Json;

namespace Ali_Mav.BlogAPI.Service.Implementation
{
    public class JsonPlaceHolderService : IJsonPlaceHolderService
    {
        private readonly HttpClient _httpClient;
        public JsonPlaceHolderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<PostCreateDto>> FetchPost()
        {
            var url = "https://jsonplaceholder.typicode.com/posts";
            //var httpClient = new HttpClient();
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                List<PostCreateDto> posts = JsonConvert.DeserializeObject<List<PostCreateDto>>(json);

                return posts;
            }

            throw new Exception();
        }

        public async Task<List<UserViewModel>> FetchUser()
        {
            var url = "https://jsonplaceholder.typicode.com/users";
            //var user = new HttpClient();
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                List<UserViewModel> users = JsonConvert.DeserializeObject<List<UserViewModel>>(json);

                return users;
            }

            throw new Exception();
        }

    }
}
