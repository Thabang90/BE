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
                await shipmentTransitService.CreateShipmentTransitAsync(request);
                return Ok(new { Message = "Shipment successfully created!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("costs/{id}")]
        public IActionResult GetShipmentCosts(int id)
        {
            return Ok(new { Price = 45, Distance = 20 });
        }
    }
}
