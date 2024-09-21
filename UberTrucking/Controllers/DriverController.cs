using Microsoft.AspNetCore.Mvc;
using UberTrucking.Services.Models;
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
                if (string.IsNullOrEmpty(results.Message))
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

        [HttpPost("create-driver-details")]
        public async Task<IActionResult> CreateDriverDetailsAsync([FromBody] DriverDetailRequest driverDetailsRequest)
        {
            try
            {
                var results = await this.driverDetailService.CreateDriverDetailAsync(driverDetailsRequest);

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

        [HttpGet("available-drivers")]
        public async Task<IActionResult> GetAllAvailableDrivers()
        {
            try
            {
                var result = await this.driverDetailService.GetAvailableDriversAsync();
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.DriverDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }

        }
    }
}
