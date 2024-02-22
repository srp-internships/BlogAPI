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
                    serviceResponse.Description = "There are already objects in the Database.";
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

        public async Task<BaseResponse<PostGetDto>> CreatePost(PostCreateDto postDto)
        {
            var serResponse = new BaseResponse<PostGetDto>();

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

                    var postGetDto = _mapper.Map<PostGetDto>(post);
                    serResponse.Data = postGetDto;
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
                var post = await _postRepository.GetById(id);
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
            
            try
            {
                var posts = _postRepository.GetAll();
                if (posts.Any() == true)
                {
                    
                    serviceResponse.success = true;
                    serviceResponse.Data = _mapper.Map<List<PostGetDto>>(posts);
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
                var dbPosts = _postRepository.GetAll().Count();
                if (pageSize <= 0 | pageSize > dbPosts)
                {
                    serviceResponse.Description = $"Pages starts from number 1 to {dbPosts}";
                    serviceResponse.success = false;
                }
                else
                {
                    var posts = await _postRepository.GetPaging(pageSize, pagenumber);
                    
                    serviceResponse.success = true;
                    serviceResponse.Data = posts;
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
                var posts = _postRepository.GetAll()
                    .Where(c => c.UserId == userid).ToList();

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
                var post = await _postRepository.GetById(updatePost.Id);
                var responseUserService = await _userService.GetById(updatePost.UserId);

                if (post != null && responseUserService.Data != null) 
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
                if (responseUserService.Data == null)
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
