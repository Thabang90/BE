
using Microsoft.AspNetCore.Mvc;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentTransitController : ControllerBase
    {
        private readonly IShipmentTransitService shipmentTransitService;

        public ShipmentTransitController(IShipmentTransitService shipmentTransitService)
        {
            this.shipmentTransitService = shipmentTransitService;
        }

        [HttpPost("create-shipment")]
        public async Task<IActionResult> CreateShimentAsync([FromBody] ShipmentTransitRequest request)
        {
            try
            {
                var results = await shipmentTransitService.CreateShipmentTransitAsync(request);
                if(results == null)
                {
                    return BadRequest("The shipment was not created. Please try again");
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-transaction")]
        public async Task<IActionResult> CreateShipmentTransactionAsync([FromBody] ShipmentTransactionRequest request)
        {
            try
            {
                await this.shipmentTransitService.CreateShipmentTransactionAsync(request);
                return Ok(new { Message = "Transaction successfully completed"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-shipment-driver")]
        public async Task<IActionResult> UpdateShipmentTransitDriverAsync([FromBody] ShipmentTransactionRequest request)
        {
            try
            {
                await this.shipmentTransitService.CreateShipmentTransactionAsync(request);
                return Ok(new { Message = "Driver Assigned to Shipment" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("available-shipments/{driverId}")]
        public async Task<IActionResult> GetAllAvailableShipmentTransitsAsync(int driverId)
        {
            try
            {
                var results = await this.shipmentTransitService.GetAvailableShipmentsAsync(driverId);
                if(!string.IsNullOrEmpty(results.ErrorMessage))
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

        [HttpGet("shipment-has-driver/{id}")]
        public async Task<IActionResult> VerifyShipmentDriverAsync(int id)
        {
            try
            {
                var result = await this.shipmentTransitService.ShipmentHasDriverAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpGet("update-shipment-transaction-cost/{id}")]
        public async Task<IActionResult> UpdateShipmentTransactionCostAsync(int id)
        {
            try
            {
                var result = await this.shipmentTransitService.UpdateShipmentTransactionUserAcceptanceCostAsync(id);
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return BadRequest(result.ErrorMessage);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }


        [HttpGet("costs/{id}")]
        public IActionResult GetShipmentCosts(int id)
        {
            return Ok(new { Price = 45, Distance = 20 });
        }
    }
}
