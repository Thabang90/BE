using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Infrastructure.Entities
{
    public class ShipmentTransaction
    {
        public int ShipmentId { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Distance { get; set; }
    }
}
