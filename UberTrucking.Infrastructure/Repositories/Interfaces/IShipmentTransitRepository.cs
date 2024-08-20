using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;

namespace UberTrucking.Infrastructure.Repositories.Interfaces
{
    public interface IShipmentTransitRepository
    {
        Task CreateShimentTransitAsync(ShipmentTransit shipmentTransit);
    }
}
