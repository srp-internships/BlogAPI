using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Ali_Mav.BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Seed()
        {
            var response = await _seedService.SeedDataBase();
            if (response.success)
            {
                return Ok(response);
            }
            return BadRequest(response.Description);
        }
    }
}
