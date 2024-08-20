using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IShipmentTransitService
    {
        Task CreateShipmentTransitAsync(ShipmentTransitRequest request);
    }
}
