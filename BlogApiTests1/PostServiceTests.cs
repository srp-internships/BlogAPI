using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Implementation;
using Ali_Mav.BlogAPI.Service.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TestProject1;

public class PostServiceTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IPostRepository> _postRepositoryMock;
    private PostService postService;

    [SetUp]
    public void SetUp()
    { 
        _userServiceMock = new Mock<IUserService>();
        _postRepositoryMock = new Mock<IPostRepository>();
        _mapperMock = new Mock<IMapper>();
        postService = new PostService(_mapperMock.Object, _postRepositoryMock.Object, _userServiceMock.Object);
    }

    [Test]
    public async Task AddPosts_WhenNoPostsExist_AddsPostsAndReturnsSuccess()
    {
        var postDtos = new List<PostCreateDto>
        {
            new PostCreateDto { Title = "Title 1", Body = "Content 1" },
            new PostCreateDto { Title = "Title 2", Body = "Content 2" }
        };
        var mappedPosts = new List<Post>
        {
            new Post { Title = "Title 1", Body = "Content 1" },
            new Post { Title = "Title 2", Body = "Content 2" }
        };
        _postRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Post>()); 
        _mapperMock.Setup(mapper => mapper.Map<List<PostCreateDto>, List<Post>>(postDtos)).Returns(mappedPosts);

        // Act
        var result = await postService.AddPosts(postDtos);

        // Assert
        Assert.IsTrue(result.success);
        Assert.AreEqual(postDtos, result.Data);
        _postRepositoryMock.Verify(repo => repo.AddRange(mappedPosts), Times.Once);
    }

    [Test]
    public async Task AddPosts_WhenPostsExist_ReturnSeccesIsFalse()
    {
        var postDtos = new List<PostCreateDto>
        {
            new PostCreateDto { Title = "Title 1", Body = "Content 1" },
            new PostCreateDto { Title = "Title 2", Body = "Content 2" }
        };
        
        var postDb = new List<Post>
        {
            new Post { Title = "Title 1", Body = "Content 1" },
            new Post { Title = "Title 2", Body = "Content 2" }
        };
        _postRepositoryMock.Setup(x => x.GetAll()).Returns(postDb);
        var result = await postService.AddPosts(postDtos);
        
        Assert.IsFalse(result.success);
        Assert.AreEqual(result.Description, "There are already objects in the Database.");
    }

    [Test]
    public async Task GetAll_WhenDatabaseFull_ReturnListPost()
    {
        var posts = new List<Post>()
        {
            new Post() { Id = 1 },
            new Post() { Id = 2 },
            new Post() { Id = 3 },
        };
        var postGetDtos = new List<PostGetDto>()
        {
            new PostGetDto() { Id = 1 },
            new PostGetDto() { Id = 2 },
            new PostGetDto() { Id = 3 },
        };
        _postRepositoryMock.Setup(x => x.GetAll()).Returns(posts);
        _mapperMock.Setup(x => x.Map<List<PostGetDto>>(posts)).Returns(postGetDtos);

        var result = await postService.GetAll();
        
        Assert.IsTrue(result.success);
        Assert.AreEqual(result.Data, postGetDtos);
    }
    
    [Test]
    public async Task GetAll_WhenDatabaseEmpty_ReturnSeccesFalseAndDataNull()
    {
        _postRepositoryMock.Setup(x => x.GetAll()).Returns(new List<Post>());
        var result = await postService.GetAll();
        
        Assert.IsTrue(result.success);
        Assert.AreEqual(result.Description, "there are 0 elements in the database");
        Assert.IsNull(result.Data);  
    }

    [Test]
    public async Task GetPaging_WhenPageSizeIsZero_ReturnSeccesIsFalse()
    {
        int pageSize = 0;
        int pageNumber = 1;

        var result = await postService.GetPaging(pageSize, pageNumber);
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
    }
    
    [Test]
    public async Task GetPaging_WhenPageSizeIsCreaterThanDbPostsCount_ReturnSeccesIsFalse()
    {
        var mockPostList = new List<Post>();
        for (int i = 0; i < 100; i++)
        {
            var post = new Post();
            post.Id = i++;
            mockPostList.Add(post);
        }

        int pageSize = 200;
        int pageNumber = 1;
        
        _postRepositoryMock.Setup(x => x.GetAll()).Returns(mockPostList);
        var result = await postService.GetPaging(pageSize, pageNumber);
        
        Assert.IsFalse(result.success);
        Assert.IsNull(result.Data);
        Assert.AreEqual(result.Description, $"Pages starts from number 1 to {mockPostList.Count()}");
    }
    
    [Test]
    public async Task GetPaging_WhenPageSizeIsSmallThanDbPostsCount_ReturnSeccesIsTrue()
    {
        var mockPostList = new List<Post>();
        for (int i = 0; i < 100; i++)
        {
            var post = new Post();
            post.Id = i++;
            mockPostList.Add(post);
        }

        int pageSize = 40;
        int pageNumber = 2;
        
        _postRepositoryMock.Setup(x => x.GetAll()).Returns(mockPostList);
        _postRepositoryMock.Setup(x => x.GetPaging(pageSize, pageNumber)).ReturnsAsync(mockPostList);
        var result = await postService.GetPaging(pageSize, pageNumber);
        
        Assert.IsTrue(result.success);
        Assert.IsNotNull(result.Data);
    }

    [Test]
    public async Task GetUserPosts_UserExists_ReturnListUser()
    {
        var userId = 1;
        var posts = new List<Post>()
        {
            new Post(){ Id = 1, UserId = 1},
            new Post(){ Id = 2, UserId = 1},
            new Post(){ Id = 3, UserId = 1},
        };

        _postRepositoryMock.Setup(x => x.GetAll()).Returns(posts);

        var result = await postService.GetUserPosts(userId);
        
        Assert.IsTrue(result.success);
        Assert.AreEqual(result.Data,posts);
    }
    
    [Test]
    public async Task GetUserPosts_UserNotFound_ReturnSeccesIsFalseAndDataIsNull()
    {
        var userId = 2;
        var posts = new List<Post>()
        {
            new Post(){ Id = 1, UserId = 1},
            new Post(){ Id = 2, UserId = 1},
            new Post(){ Id = 3, UserId = 1},
        };

        _postRepositoryMock.Setup(x => x.GetAll()).Returns(posts);

        var result = await postService.GetUserPosts(userId);
        
        Assert.IsFalse(result.success);
        Assert.AreNotEqual(result.Data,posts);
        Assert.AreEqual(result.Description, $"There are 0 elements in the database with id = {userId}");
        Assert.IsNull(result.Data);
    }

    [Test]
    public async Task CreatePost_SetPostCreatDto_ReturnPostGetDto()
    {
        // Arrange
        var userId = 1;

        var user = new User() { Id = userId };

        var post = new Post()
        {
            Id = 1,
            Body = "abc",
            Title = "dfg"
        };
        var creatdto = new PostCreateDto()
        { 
            UserId = userId, 
            Body = "abc", 
            Title = "dfg" 
        };

        var getDto = new PostGetDto()
        {
            UserId = userId, 
            Body = "abc", 
            Title = "dfg", 
            Id = 5 
        };

        _userServiceMock.Setup(x => x.GetById(userId))
                                    .ReturnsAsync(new BaseResponse<User>() { Data = user });
        
        _postRepositoryMock.Setup(x => x.Create(It.IsAny<Post>()))
                                    .Callback<Post>(p => p.Id = getDto.Id); 

        _mapperMock.Setup(x => x.Map<PostGetDto>(It.IsAny<Post>())).Returns(getDto);

       
        var result = await postService.CreatePost(creatdto);

       
        Assert.IsTrue(result.success);
        Assert.AreEqual(getDto, result.Data);
    }
    
    [Test]
    public async Task CreatePost_SetPostCreatDtoButUserNotFound_ReturnSeccesIsFalseDataIsNull()
    {
        // Arrange
        var userId = 4;

        var user = new User()
        {
            Id = 1
        };

        var post = new Post()
        {
            Id = 1,
            Body = "abc",
            Title = "dfg"
        };
        var creatdto = new PostCreateDto()
        { 
            UserId = userId, 
            Body = "abc", 
            Title = "dfg" 
        };

        var getDto = new PostGetDto()
        {
            UserId = 1, 
            Body = "abc", 
            Title = "dfg", 
            Id = 5 
        };

        _userServiceMock.Setup(x => x.GetById(userId))
                                    .ReturnsAsync(new BaseResponse<User>() { Data = null });
        
        var result = await postService.CreatePost(creatdto);

       
        Assert.IsFalse(result.success);
        Assert.AreEqual("User not fount", result.Description);
    }

    [Test]
    public async Task UppDatePost_SetPostUpdateDtoUserExist_ReturnSeccesAndPostGetDto()
    {
        var updatePost = new PostUpdateDto()
        {
            Id = 2,
            UserId = 1,
            Body = "fgh",
            Title = "abc"
        };
        var newPost = new Post()
        {
            Id = 2,
            UserId = 1,
            Body = "fgh",
            Title = "abc"
        };
        var existingPost = new Post
        {
            Id = 1,
            UserId = 1,
            Title = "Original Title",
            Body = "Original Body"
        };
        var getPost = new PostGetDto()
        {
            Id = 2,
            UserId = 3,
            Body = "qqq",
            Title = "sss"
        };
        var user = new User()
        {
            Id = 1
        };
        _postRepositoryMock.Setup(x => x.GetById(updatePost.Id)).ReturnsAsync(existingPost);
        _userServiceMock.Setup(x => x.GetById(updatePost.UserId)).ReturnsAsync(new BaseResponse<User>(){Data = user});
        _postRepositoryMock.Setup(x => x.Update(It.IsAny<Post>())).ReturnsAsync(newPost);
        _mapperMock.Setup(x => x.Map<PostGetDto>(It.IsAny<Post>())).Returns(getPost);
        
        var result = await postService.UpdatePost(updatePost);
        
        Assert.IsTrue(result.success);
        Assert.AreEqual(result.Data, getPost);

    }

    [Test]
    public async Task UppDatePost_SetPostUpdateDtoButUserNotExist_ReturnSeccesIsFalse()
    {
        var updatePost = new PostUpdateDto()
        {
            Id = 2,
            UserId = 5,
            Body = "fgh",
            Title = "abc"
        };
        var existingPost = new Post()
        {
            Id = 2,
            UserId = 1,
            Title = "Original Title",
            Body = "Original Body"
        };
        
        _postRepositoryMock.Setup(x => x.GetById(updatePost.Id)).ReturnsAsync(existingPost);
        _userServiceMock.Setup(x => x.GetById(updatePost.UserId)).ReturnsAsync(new BaseResponse<User>(){Data = null});
        
        var result = await postService.UpdatePost(updatePost);
        
        Assert.IsFalse(result.success);
        Assert.AreEqual(result.Data, null);
        Assert.AreEqual(result.Description, "UserId not found");

    } 
    [Test]
    public async Task UppDatePost_SetPostUpdateDtoButPostNotExist_ReturnSeccesIsFalse()
    {
        var updatePost = new PostUpdateDto()
        {
            Id = 2,
            UserId = 5,
            Body = "fgh",
            Title = "abc"
        };
        var existingPost = new Post()
        {
            Id = 2,
            UserId = 1,
            Title = "Original Title",
            Body = "Original Body"
        };
        
        _postRepositoryMock.Setup(x => x.GetById(updatePost.Id)).ReturnsAsync(() => null);
        
        _userServiceMock.Setup(x => x.GetById(updatePost.UserId)).ReturnsAsync(new BaseResponse<User>()
                                                            {Data = new User(){Id = 5}});
        
        var result = await postService.UpdatePost(updatePost);
        
        Assert.IsFalse(result.success);
        Assert.AreEqual(result.Data, null);
        Assert.AreEqual(result.Description,"PostId not found");
    }

    [Test]
    public async Task DeletePost_WhenPostIdExist_ReturnTrue()
    {
        var post = new Post()
        {
            Id = 5
        };
        _postRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(post);

        var result = await postService.DeletePost(post.Id);
        
        Assert.IsTrue(result.success);
        Assert.IsFalse(result.Data);
    }
    
    [Test]
    public async Task DeletePost_WhenPostIdNotExist_ReturnFalse()
    {
        var post = new Post()
        {
            Id = 5
        };
        _postRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(()=> null);

        var result = await postService.DeletePost(post.Id);
        
        Assert.IsFalse(result.success);
        Assert.IsFalse(result.Data);
        Assert.AreEqual(result.Description, "Post not found");
    }
}