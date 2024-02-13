using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ali_Mav.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeadController : ControllerBase
    {
        private readonly IPostService _postService;

        public SeadController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<BaseResponse<Post>>> Create(PostCreateDto postDto)
        {
            var response = await _postService.CreatePost(postDto);
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("AddPosts")]
        public async Task<ActionResult<BaseResponse<List<Post>>>> AddAllPosts()
        {
            var response = await _postService.CreateAll();
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<BaseResponse<List<PostGetDto>>>> GetAll() 
        {
            var response = await _postService.GetAll();
            if (response.success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }

        [HttpGet("GetUserPosts")]
        public async Task<ActionResult<List<Post>>> GetUserPosts(int userId)
        {
            var response = await _postService.GetUserPosts(userId);

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<BaseResponse<Post>>> Update(PostUpdateDto postDto)
        {
            var response = await _postService.UpdatePost(postDto);
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<BaseResponse<bool>>> Delete(int id)
        {
            var response =await _postService.DeletePost(id);
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("Page")]
        public async Task<ActionResult<BaseResponse<List<Post>>>> GetPaging(int pageSize, int pagenumber)
        { 
            var response = await _postService.GetPaging(pageSize, pagenumber);
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
