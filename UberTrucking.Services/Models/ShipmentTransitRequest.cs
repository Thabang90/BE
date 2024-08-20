using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class ShipmentTransitRequest
    {
        public string PickupAddress { get; set; }
        public decimal PickupLatitude { get; set; }
        public decimal PickupLongitude { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal DeliveryLatitude { get; set; }
        public decimal DeliveryLongitude { get; set; }
    }
}
