using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class DriverPositionRequest
    {
        public int DriverId { get; set; }
        public int ShipmentId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Waypoint { get; set; }
    }
}
