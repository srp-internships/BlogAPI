using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ali_Mav.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<List<User>>>> AddUsers()
        {
            var response = await _userService.CreateAll();
            
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Search(string word)
        { 
            var response = await _userService.Search(word);
            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        
        }


    }
}
