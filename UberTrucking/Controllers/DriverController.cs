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

        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var result = await this.driverDetailService.GetAllDriversAsync();
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.DriverDetails);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdfDocumentAsync(FileUploadRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest("No file uploaded!");
                }

                var result = await this.driverDetailService.UploadPdfDocumentAsync(request);
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return NotFound(result.ErrorMessage);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadPdfAsync(string fileName)
        { 
            var result = await this.driverDetailService.DownloadPdfAsync(fileName);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return NotFound(result.ErrorMessage); 
            }

            return File(result.DownloadedFile.Content, "application/pdf", result.DownloadedFile.FileName);
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

        [HttpPost("{driverId}/activate")]
        public async Task<IActionResult> ActivateDriverAsync(int driverId)
        {
            try
            {
                await this.driverDetailService.ActivateDriverByIdAsync(driverId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpPost("{driverId}/deactivate")]
        public async Task<IActionResult> DeactivateDriverAsync(int driverId)
        {
            try
            {
                await this.driverDetailService.DeactivateDriverByIdAsync(driverId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpPost("upsert-driver-position")]
        public async Task<IActionResult> UpsertDriverPosition([FromBody] DriverPositionRequest driverPositionRequest)
        {
            try
            {
                var results = await this.driverDetailService.UpsertDriverPositionAsync(driverPositionRequest);

                if (!string.IsNullOrEmpty(results.ErrorMessage))
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

        [HttpGet("retrieve-driver-position/{driverId}")]
        public async Task<IActionResult> GetDriverPositionAsync(int driverId)
        {
            try
            {
                var results = await this.driverDetailService.GetDriverPositionAsync(driverId);

                if (!string.IsNullOrEmpty(results.ErrorMessage))
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
