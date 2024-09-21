using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class ShipmentTransactionRequest
    {
        public int Id { get; set; }
        public int ShipmentId {  get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Distance { get; set; }
    }
}
