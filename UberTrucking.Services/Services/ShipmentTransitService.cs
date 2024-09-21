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
                AddressData = request.AddressData,
                UserId = request.UserId,
                Height = request.Height,
                Width = request.Width,
                Length = request.Length, 
                Description = request.Description
            };

            await this.shipmentTransitRepository.CreateShimentTransitAsync(shipmentTransit);
        }

        public async Task CreateShipmentTransactionAsync(ShipmentTransactionRequest shipmentTransactionRequest)
        {
            var shipmentTransaction = new ShipmentTransaction()
            {
                ShipmentId = shipmentTransactionRequest.ShipmentId,
                Price = shipmentTransactionRequest.Price,
                Distance = shipmentTransactionRequest.Distance,
                PaymentMethod = shipmentTransactionRequest.PaymentMethod,
            };

            await this.shipmentTransitRepository.CreateShipmentTransactionAsync(shipmentTransaction); 
        }

        public async Task UpdateShipmentDriverAsync(UpdateShipmentDriverRequest request)
        {
            await this.shipmentTransitRepository.UpdateShipmentDriverAsync(request.ShipmentId, request.ShipmentId);
        }
    }
}
