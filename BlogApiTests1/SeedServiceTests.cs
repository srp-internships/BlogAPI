using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Implementation;
using Ali_Mav.BlogAPI.Service.Interface;
using Moq;

namespace TestProject1;

  public class SeedServiceTests
    {
        private Mock<IUserService> _userService;
        private Mock<IJsonPlaceHolderService> _jsonPlaceHolderService;
        private Mock<IPostService> _postService;
        private ISeedService _seedService;

        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
            _jsonPlaceHolderService = new Mock<IJsonPlaceHolderService>();
            _postService = new Mock<IPostService>();

            _seedService = new SeedService(
                _jsonPlaceHolderService.Object,
                _userService.Object,
                _postService.Object);
        }

        [Test]
        public async Task SeedDataBase_WhenNoDataInDatabase_ReturnServiceResponseSuccessIsTrue()
        {
            _jsonPlaceHolderService.Setup(x => x.FetchPost()).ReturnsAsync(new List<PostCreateDto>());
            _jsonPlaceHolderService.Setup(x => x.FetchUser()).ReturnsAsync(new List<UserViewModel>());
            _userService.Setup(x => x.GetAll()).ReturnsAsync(new BaseResponse<List<User>> { Data = new List<User>() });

            var result = await _seedService.SeedDataBase();

            Assert.That(result.success, Is.True);

        }
        
        [Test]
        public async Task SeedDataBase_WhenWhereIsAlreadyDataInTheDatabase_ReturnServiceResponseSuccessIsFalse()
        {
            var users = new List<User>
            {
                new User{ Id = 1, Address = "35555"},
                new User{ Id = 2, Address = "35555sdcsdcsdcds"},

            };

            _jsonPlaceHolderService.Setup(x => x.FetchPost()).ReturnsAsync(new List<PostCreateDto>());
            _jsonPlaceHolderService.Setup(x => x.FetchUser()).ReturnsAsync(new List<UserViewModel>());
            _userService.Setup(x => x.GetAll()).ReturnsAsync(new BaseResponse<List<User>> { Data = users });

            var result = await _seedService.SeedDataBase();

            Assert.That(result.success, Is.False);
        }
    }
