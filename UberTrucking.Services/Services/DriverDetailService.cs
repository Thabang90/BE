using Org.BouncyCastle.Asn1.Crmf;
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

        public async Task<DriverDetailResponse> CreateDriverDetailAsync(DriverDetailRequest driverDetailRequest)
        {
            var response = new DriverDetailResponse();
            if(driverDetailRequest == default(DriverDetailRequest))
            {
                response.ErrorMessage = "Driver details missing, please ensure all details are populated!";
            }

            var driverDetail = new DriverDetail()
            {
                DriverId = driverDetailRequest.DriverId,
                VehicleRegistration = driverDetailRequest.VehicleRegistration,
                VehicleMake = driverDetailRequest.VehicleMake,
                VehicleModel = driverDetailRequest.VehicleModel,
                IsAvailable = driverDetailRequest.IsAvailable
            };

            await this.driverDetailRepository.CreateDriverDetailAsync(driverDetail);
            response.Message = "Driver details have been added successfully!";
            return response;
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
        public async Task<DriverDetailResponse> GetAvailableDriversAsync()
        {
            var response = new DriverDetailResponse();
            var result = await this.driverDetailRepository.GetAvailableDriversAsync();
            if (result.Count > 0)
            {
                response.DriverDetails = result;
            }
            else
            {
                response.ErrorMessage = "No Drivers available at the moment, please try again in a few minutes!";
            }

            return response;
        }
    }

}
