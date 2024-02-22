using System.Net;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Service.Implementation;
using Moq;
using Newtonsoft.Json;

namespace TestProject1;

[TestFixture]
    public class JsonPlaceHolderServiceTests
    {
        private Mock<IHttpClientWrapper> _httpClientMock;
        private JsonPlaceHolderService _jsonPlaceHolder;

        [SetUp]
        public void SetUp()
        { 
            _httpClientMock = new Mock<IHttpClientWrapper>();
            _jsonPlaceHolder = new JsonPlaceHolderService(_httpClientMock.Object);
        }

        [Test]
        public async Task FetchPost_WhenSuccess_ReturnPosts()
        {
            var postsJson = "[{ 'userId': 1, 'id': 1, 'title': 'Post 1', 'body': 'Body 1' }]";
            var expectedPosts = JsonConvert.DeserializeObject<List<PostCreateDto>>(postsJson);

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(postsJson)
            });

            var result = await _jsonPlaceHolder.FetchPost();

            Assert.That(result.Count, Is.EqualTo(expectedPosts.Count));

        }
        
        [Test]
        public async Task FetchPost_WhenIsNotSuccess_ReturnException()
        {
            var postsJson = "[{ 'userId': 1, 'id': 1, 'title': 'Post 1', 'body': 'Body 1' }]";
            var expectedPosts = JsonConvert.DeserializeObject<List<PostCreateDto>>(postsJson);

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            });

            Assert.ThrowsAsync<Exception>(() => _jsonPlaceHolder.FetchPost());
        }
        
        [Test]
        public async Task FetchUser_WhenSuccess_ReturnUserViewModels()
        {
            var postsJson = "[{ 'userId': 1, 'id': 1, 'title': 'Post 1', 'body': 'Body 1' }]";
            var expectedPosts = JsonConvert.DeserializeObject<List<PostCreateDto>>(postsJson);

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(postsJson)
            });

            var result = await _jsonPlaceHolder.FetchUser();

            Assert.That(result.Count, Is.EqualTo(expectedPosts.Count));

        }
        
        [Test]
        public async Task FetchUser_WhenIsNotSuccess_ReturnException()
        {
            var postsJson = "[{ 'userId': 1, 'id': 1, 'title': 'Post 1', 'body': 'Body 1' }]";
            var expectedPosts = JsonConvert.DeserializeObject<List<PostCreateDto>>(postsJson);

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            });

            Assert.ThrowsAsync<Exception>(() => _jsonPlaceHolder.FetchUser());
        }
    }
