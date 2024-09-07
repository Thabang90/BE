using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Infrastructure.Entities
{
    public  class ShipmentTransit
    {
        public int Id { get; set; }
        public string PickupAddress { get; set; }
        public decimal PickupLatitude { get; set; }
        public decimal PickupLongitude { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal DeliveryLatitude { get; set; }
        public decimal DeliveryLongitude { get; set; }
        public string? AddressData { get; set; }
    }
}
