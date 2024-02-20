using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Service.Implementation;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApiTests
{
    [TestFixture]
    public class JsonPlaceHolderServiceTests
    {
        private Mock<HttpClient> _httpClientMock;
        private JsonPlaceHolderService _jsonPlaceHolder;

        [SetUp]
        public void SetUp()
        { 
            _httpClientMock = new Mock<HttpClient>();
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
                Content = new StringContent(postsJson),
                
            });

            var result = await _jsonPlaceHolder.FetchPost();


            Assert.That(result.Count, Is.EqualTo(postsJson.Length));

        }


    }
}
