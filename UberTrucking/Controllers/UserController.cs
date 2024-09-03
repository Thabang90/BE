using Microsoft.AspNetCore.Mvc;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request)
        {
            try
            {
                var results = await userService.CreateUserAsync(request);
                if(string.IsNullOrEmpty(results.Message))
                {
                    return BadRequest(results.ErrorMessage);
                }

                return Ok(results.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}
