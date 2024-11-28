using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;

namespace UberTrucking.Services.Models
{
    public class ShipmentWithDriverResponse
    {
        public bool HasDriver { get; set; }
        public ShipmentTransit ShipmentTransit { get; set; }
        public string Message { get; set; } 
        public string ErrorMessage { get; set; }
    }
}
