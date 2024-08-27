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
        public required string AddressData { get; set; }
    }
}
