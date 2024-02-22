using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Service.Implementation;
using AutoMapper;
using Moq;

namespace BlogApiTests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }


        [Test]
        public async Task AddUsers_NoUsersInDatabase_AddsUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var usersToAdd = new List<UserViewModel>
        {
            new UserViewModel { /* user properties */ },
            new UserViewModel { /* another user properties */ }
            // Add more users as needed
        };

            var userList = mapperMock.Setup(m => m.Map<List<UserViewModel>, List<User>>(usersToAdd))
                                    .Returns(It.IsAny<List<User>>());

            // userRepositoryMock.Setup(repo => repo.GetAll()).Returns();
            userRepositoryMock.Setup(repo => repo.AddUsers(It.IsAny<List<User>>())).Verifiable();

            // Act
            var result = await _service.AddUsers(usersToAdd);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Description);
            // Assert.AreEqual(usersToAdd, result.Data);

            userRepositoryMock.Verify(repo => repo.AddUsers(It.IsAny<List<User>>()), Times.Once);
        }

    }
}
