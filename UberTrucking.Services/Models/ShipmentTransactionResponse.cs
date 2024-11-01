using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;

namespace UberTrucking.Services.Models
{
    public class ShipmentTransactionResponse
    {
        public ShipmentTransaction ShipmentTransaction { get; set; }
        public string ErrorMessage { get; set; }
    }
}
