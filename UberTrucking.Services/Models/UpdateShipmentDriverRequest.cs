using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class UpdateShipmentDriverRequest
    {
        public int ShipmentId { get; set; }
        public int DriverId { get; set; }
    }
}
