using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Ali_Mav.BlogAPI.Service.Implementation
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;

        public PostService(IMapper mapper, IPostRepository postRepository, IUserService userService)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userService = userService;

        }

        public async Task<BaseResponse<List<PostCreateDto>>> AddPosts(List<PostCreateDto> postDtos)
        {
            var serviceResponse = new BaseResponse<List<PostCreateDto>>();
            try
            {
                var postDb = _postRepository.GetAll();
                if (!postDb.Any())
                {
                    var postList = _mapper.Map<List<PostCreateDto>, List<Post>>(postDtos);
                    await _postRepository.AddRange(postList);

                    serviceResponse.success = true;
                    serviceResponse.Data = postDtos;
                }
                else
                {
                    serviceResponse.success = false;
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Description = ex.Message;
                serviceResponse.success = false;
            }
            return serviceResponse;
        }

        public async Task<BaseResponse<Post>> CreatePost(PostCreateDto postDto)
        {
            var serResponse = new BaseResponse<Post>();

            try
            {
                var user = await _userService.GetById(postDto.UserId);
                if (user.Data != null)
                {
                    var post = new Post()
                    {
                        Body = postDto.Body,
                        Title = postDto.Title,
                        UserId = postDto.UserId,
                        User = user.Data
                    };

                    await _postRepository.Create(post);
                    
                    serResponse.Data = post;
                    serResponse.success = true;
                }
                else
                {
                    serResponse.success = false;
                    serResponse.Description = "User not fount";
                }
            }
            catch (Exception ex)
            {
                serResponse.Description = ex.Message;
                serResponse.success = false;
            }
                return serResponse;
        }

        public async Task<BaseResponse<bool>> DeletePost(int id)
        {
            var serviceResponse = new BaseResponse<bool>();
            try
            {
                var post = await _postRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
                if (post != null)
                {
                    await _postRepository.Delete(id);

                    serviceResponse.success = true;
                    serviceResponse.Description = "seccess";
                }
                else
                {
                    serviceResponse.success = false;
                    serviceResponse.Description = "Post not found";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.Description = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<BaseResponse<List<PostGetDto>>> GetAll()
        {
            var serviceResponse = new BaseResponse<List<PostGetDto>>();
            List<PostGetDto> postDto = new List<PostGetDto>();
            try
            {
                var posts = await _postRepository.GetAll()
                    .Include(c=>c.User)
                    .ToListAsync();

                if (posts.Any() == true)
                {
                    foreach (var item in posts)
                    {
                        postDto.Add(_mapper.Map<PostGetDto>(item));
                    }

                    serviceResponse.success = true;
                    serviceResponse.Data = postDto;
                }
                else
                {
                    serviceResponse.success = true;
                    serviceResponse.Description = "there are 0 elements in the database";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.Description = ex.Message;
            }
            return serviceResponse;

        }

        public async Task<BaseResponse<List<Post>>> GetPaging(int pageSize, int pagenumber)
        {
            var serviceResponse = new BaseResponse<List<Post>>();
            try
            {
                var dbPosts = await _postRepository.GetAll().ToListAsync();
                if (pageSize <= 0 | pageSize > dbPosts.Count | pagenumber < dbPosts.Count/pageSize)
                {
                    serviceResponse.Description = $"Pages starts from number 1 to {dbPosts.Count} " +
                        $"and the page number should be no more than {(dbPosts.Count / pageSize) + 1}";
                    serviceResponse.success = false;
                }
                else
                {
                    var posts = await _postRepository.GetAll()
                        .Skip(pageSize*(pagenumber-1))
                        .Take(pageSize)
                        .ToListAsync();
                    if (posts.Count >= 0)
                    {
                        serviceResponse.success = true;
                        serviceResponse.Data = posts;
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.Description = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<BaseResponse<List<Post>>> GetUserPosts(int userid)
        {
            var seviceResponse = new BaseResponse<List<Post>>();
            try
            {
                var posts = await _postRepository.GetAll()
                    .Where(c => c.UserId == userid).ToListAsync();

                if (posts.Any() == false)
                {
                    seviceResponse.success = false;
                    seviceResponse.Description = $"There are 0 elements in the database with id = {userid}";
                }
                else
                {
                    seviceResponse.success= true;
                    seviceResponse.Data = posts;
                }
            }
            catch (Exception ex)
            {
                seviceResponse.success = false;
                seviceResponse.Description = ex.Message;
            }
            return seviceResponse;
        }

        public async Task<BaseResponse<PostGetDto>> UpdatePost(PostUpdateDto updatePost)
        {
            var serviceResponse = new BaseResponse<PostGetDto>();
            try
            {
                var post = await _postRepository.GetAll().FirstOrDefaultAsync(p => p.Id == updatePost.Id);
                var responseUserService = await _userService.GetById(updatePost.UserId);

                if (post != null && responseUserService != null) 
                {
                    var upNewPost = new Post()
                    {
                        Id = updatePost.Id,
                        User = responseUserService.Data,
                        UserId = updatePost.UserId,
                        Body = updatePost.Body,
                        Title = updatePost.Title,
                    };

                    var upPost = await _postRepository.Update(upNewPost);

                    var dto = _mapper.Map<PostGetDto>(upPost);

                    serviceResponse.success = true;
                    serviceResponse.Data = dto;
                }
                if (post == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.Description = "PostId not found";
                }
                if (responseUserService == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.Description = "UserId not found";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Description = ex.Message;
                serviceResponse.success = false;
            }
            return serviceResponse;
        }
    }
}
