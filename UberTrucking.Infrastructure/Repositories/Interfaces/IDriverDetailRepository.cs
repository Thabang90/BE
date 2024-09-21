using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;

namespace UberTrucking.Infrastructure.Repositories.Interfaces
{
    public interface IDriverDetailRepository
    {
        Task<DriverDetail> GetDriverDetailsAsync(int driverId);
        Task CreateDriverDetailAsync(DriverDetail driverDetail);

        Task UpdateDriverStatusAsync(int driverId, bool isAvailable);

        Task<List<DriverDetail>> GetAvailableDriversAsync();
    }
}
