using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IShipmentTransitService
    {
        Task<ShipmentTransitResponse> CreateShipmentTransitAsync(ShipmentTransitRequest request);
        Task CreateShipmentTransactionAsync(ShipmentTransactionRequest shipmentTransactionRequest);
        Task UpdateShipmentDriverAsync(UpdateShipmentDriverRequest request);
        Task<ShipmentTransitResponse> GetAvailableShipmentsAsync(int driverId);
        Task<ShipmentWithDriverResponse> ShipmentHasDriverAsync(int shipmentId);
        Task<ShipmentTransactionResponse> UpdateShipmentTransactionUserAcceptanceCostAsync(int shipmentId);
        Task<ShipmentTransitResponse> GetUserShipmentTransitsAsync(int userId);
    }
}
