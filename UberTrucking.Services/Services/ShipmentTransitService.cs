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
        private readonly IDriverDetailRepository driverDetailRepository;

        public ShipmentTransitService(
            IShipmentTransitRepository shipmentTransitRepository,
            IDriverDetailRepository driverDetailRepository)
        {
            this.shipmentTransitRepository = shipmentTransitRepository;
            this.driverDetailRepository = driverDetailRepository;
        }

        public async Task<ShipmentTransitResponse> CreateShipmentTransitAsync(ShipmentTransitRequest request)
        {
            var response = new ShipmentTransitResponse();
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

           response.ShipmentTransit = await this.shipmentTransitRepository.CreateShimentTransitAsync(shipmentTransit);
           return response;
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

        public async Task<ShipmentTransitResponse> GetAvailableShipmentsAsync(int driverId)
        {
            var response = new ShipmentTransitResponse();

            var isActivated = await this.driverDetailRepository.VerifyDriverActivatedStatusAsync(driverId);

            if(!isActivated)
            {
                response.ErrorMessage = "Driver is not yet Active!";
            }

            var results = await this.shipmentTransitRepository.GetAvailableShipmentsAsync();
            if(results.Any())
            {
                response.ShipmentTransits = results;
            }
            else
            {
                response.ErrorMessage = "No Shipments available at the moment!";
            }
            return response;
        }

        public async Task<bool> ShipmentHasDriverAsync(int shipmentId)
        {
            var result = await this.shipmentTransitRepository.ShipmentHasDriverAsync(shipmentId);
            return result;
        }

        public async Task<ShipmentTransactionResponse> UpdateShipmentTransactionUserAcceptanceCostAsync(int shipmentId)
        {
            var response = new ShipmentTransactionResponse();
            var result = await this.shipmentTransitRepository.UpdateShipmentTransactionUserAcceptanceCostAsync(shipmentId);
            if(result == null)
            {
                response.ErrorMessage = "No shipment transaction was found!";
            }

            response.ShipmentTransaction = result;
            return response;
        }
    }
}
