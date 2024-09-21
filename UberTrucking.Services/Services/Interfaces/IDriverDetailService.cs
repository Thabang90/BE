using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IDriverDetailService
    {
        Task<DriverDetailResponse> CreateDriverDetailAsync(DriverDetailRequest driverDetailRequest);
        Task<DriverDetailResponse> GetDriverDetailsById(int driverId);
        Task<DriverDetailResponse> GetAvailableDriversAsync();
    }
}
