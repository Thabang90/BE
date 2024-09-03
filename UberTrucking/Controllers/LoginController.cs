using Microsoft.AspNetCore.Mvc;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;

        public LoginController(IUserService userService)
        { 
            this.userService = userService;
        }

        [HttpPost] 
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                var result = await this.userService.LoginAsync(request);
                if (result.User != null)
                {
                    return Ok(new
                    {
                        Message = "Login Successful",
                        User = result.User
                    });
                }
                else
                {
                    return BadRequest(new { Message = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendResetPasswordEmailAsync(string email)
        {
            try
            {
                var results = await this.userService.SendResetLinkAsync(email);
                if (string.IsNullOrEmpty(results.Message))
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
