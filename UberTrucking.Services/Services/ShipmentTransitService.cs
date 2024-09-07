using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Services.Services
{
    public class ShipmentTransitService : IShipmentTransitService
    {
        private readonly IShipmentTransitRepository shipmentTransitRepository;

        public ShipmentTransitService(IShipmentTransitRepository shipmentTransitRepository)
        {
            this.shipmentTransitRepository = shipmentTransitRepository;
        }

        public async Task CreateShipmentTransitAsync(ShipmentTransitRequest request)
        {
            var shipmentTransit = new ShipmentTransit()
            {
                PickupAddress = request.PickupAddress,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryLatitude = request.DeliveryLatitude,
                DeliveryLongitude = request.DeliveryLongitude,
                AddressData = request.AddressData
            };

            await this.shipmentTransitRepository.CreateShimentTransitAsync(shipmentTransit);
        }
    }
}
