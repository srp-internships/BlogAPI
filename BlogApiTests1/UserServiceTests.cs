using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Service.Implementation;
using AutoMapper;
using Moq;

namespace TestProject1;

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
    public async Task GetById_IdExists_ReturnUser()
    {
        int userId = 1;
        var user = new User()
        {
            Id = 1
        };
        _userRepositoryMock.Setup((x => x.GetById(userId))).ReturnsAsync(user);

        var result = await _service.GetById((userId));
        
        Assert.IsTrue((result.success));
        Assert.AreEqual(user, result.Data);
    }
    
    [Test]
    public async Task GetById_IdDoesNotExists_ReturnDataNullAntDescriptionUserNotFound()
    {
        int userId = 2;
        User? user = null;
        _userRepositoryMock.Setup(x => x.GetById(userId))!.ReturnsAsync(user);

        var result = await _service.GetById((userId));
        
        Assert.IsTrue((result.success));
        Assert.AreEqual(result.Description, "User not found");
        Assert.IsNull(result.Data);
    }
    
    [Test]
    public async Task GetById_WhenRepositoryThrowsException_ReturnError()
    {
        int userId = 2;
        _userRepositoryMock.Setup(x => x.GetById(userId))!.ThrowsAsync(new Exception("DataBase error"));
        
        var result = await _service.GetById((userId));
        
        Assert.IsFalse((result.success));
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, "DataBase error");
    }
    
    [Test]
    public async Task GetAll_DbIsNotEmpty_ReturnListUser()
    {
        var users = new List<User>()
        {
            new User(){ Id = 1},
            new User(){Id = 2}
        };
        _userRepositoryMock.Setup(x => x.GetAll()).Returns(users);

        var result = await _service.GetAll();
        Assert.IsTrue(result.success);
        Assert.IsNotNull(result.Data);
    }
    
    [Test]
    public async Task GetAll_DbIsEmpty_ReturnListEmpty()
    {
        var users = new List<User>();
        _userRepositoryMock.Setup(x => x.GetAll()).Returns(users);
        
        var result = await _service.GetAll();
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, "Users not found");
    }
    
    [Test]
    public async Task GetAll_WhenRepositoryThrowsException_ReturnError()
    {
        _userRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception("Darabase error"));

        var result = await _service.GetAll();
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, "Darabase error");
    }
    
    [Test]
    public async Task AddUsers_WhenSetListUserViewModelAndDbIsEmpty_ReturnListUserViewModel()
    {
        var userViewModels = new List<UserViewModel>()
        {
            new UserViewModel() { Id = 1 },
            new UserViewModel() { Id = 2 },
            new UserViewModel() { Id = 3 },
        };
        var usersDb = new List<User>();
        _userRepositoryMock.Setup(x => x.GetAll()).Returns(usersDb);

        var result = await _service.AddUsers(userViewModels);
        
        Assert.IsTrue(result.success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(result.Data, userViewModels);
    }
    
    [Test]
    public async Task AddUsers_WhenSetListUserViewModelAndDbIsNotEmpty_ReturnSuccessFalseDataNull()
    {
        var userViewModels = new List<UserViewModel>()
        {
            new UserViewModel() { Id = 5 },
            new UserViewModel() { Id = 6 },
            new UserViewModel() { Id = 7 },
        };
        var usersDb = new List<User>()
        {
            new User() { Id = 1 },
            new User() { Id = 2 },
        };
        _userRepositoryMock.Setup(x => x.GetAll()).Returns(usersDb);

        var result = await _service.AddUsers(userViewModels);
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, "There is already a user in the database");
    }
    
    [Test]
    public async Task AddUsers_WhenRepositoryThrowsException_ReturnError()
    {
        var userViewModels = new List<UserViewModel>()
        {
            new UserViewModel() { Id = 5 },
            new UserViewModel() { Id = 6 },
            new UserViewModel() { Id = 7 },
        };
        _userRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception("Darabase error"));

        var result = await _service.AddUsers(userViewModels);
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, "Darabase error");
    }
}
