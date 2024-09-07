using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Services.Services
{
    public class DriverDetailService : IDriverDetailService
    {
        private readonly IDriverDetailRepository driverDetailRepository;

        public DriverDetailService(IDriverDetailRepository driverDetailRepository)
        {
            this.driverDetailRepository = driverDetailRepository;
        }

        public async Task<DriverDetailResponse> GetDriverDetailsById(int driverId)
        {
            var response = new DriverDetailResponse();
            var result = await this.driverDetailRepository.GetDriverDetailsAsync(driverId);
            if (result == null)
            {
                response.ErrorMessage = "No driver details was found! Please try again";
            }
            else
            {
                response.DriverDetail = result;
                response.Message = "Successfull";
            }
            
            return response;
        }
    }
}
