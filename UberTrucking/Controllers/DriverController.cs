using Microsoft.AspNetCore.Mvc;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverDetailService driverDetailService;

        public DriverController(IDriverDetailService driverDetailService)
        {
            this.driverDetailService = driverDetailService;
        }

        [HttpGet("drivers/{id}")]

        public async Task<IActionResult> GetDriverDetailsAsync(int id)
        {
            try
            {
                var results = await this.driverDetailService.GetDriverDetailsById(id);
                if(string.IsNullOrEmpty(results.Message))
                {
                    return BadRequest(results.ErrorMessage);
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}
