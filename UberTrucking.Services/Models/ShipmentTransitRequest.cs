using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class ShipmentTransitRequest
    {
        public required string PickupAddress { get; set; }
        public required decimal PickupLatitude { get; set; }
        public required decimal PickupLongitude { get; set; }
        public required string DeliveryAddress { get; set; }
        public required decimal DeliveryLatitude { get; set; }
        public decimal DeliveryLongitude { get; set; }
        public string AddressData { get; set; }
        public int UserId { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public string Description { get; set; }
    }
}
