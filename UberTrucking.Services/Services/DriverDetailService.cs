using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IDriverPositionRepository driverPositionRepository;
        private readonly IConfiguration config;

        public DriverDetailService(IDriverDetailRepository driverDetailRepository, IConfiguration config, IDriverPositionRepository driverPositionRepository)
        {
            this.driverDetailRepository = driverDetailRepository;
            this.config = config;
            this.driverPositionRepository = driverPositionRepository;
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

        public async Task<DriverDetailResponse> GetAllDriversAsync()
        {
            var response = new DriverDetailResponse();
            var result = await this.driverDetailRepository.GetAllDriversAsync();

            if (result.Count() > 0)
            {
                response.DriverDetails = result;
            }
            else
            {
                response.ErrorMessage = "There are no drivers created on the system!";
            }

            return response;
        }

        public async Task<FileUploadResponse> UploadPdfDocumentAsync(FileUploadRequest request)
        {
            var response = new FileUploadResponse();
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!request.File.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                response.ErrorMessage = "Only PDF files are allowed!";
                return response;
            }

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            string filePath = Path.Combine(uploadDirectory, request.DriverId + ".pdf");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            response.Message = "File uploaded successfully";

            return response;
        }

        public async Task<FileUploadResponse> DownloadPdfAsync(string fileName)
        {
            var response = new FileUploadResponse();
            try
            {
                response = new FileUploadResponse();
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadDirectory))
                {
                    response.ErrorMessage = "Directory Not Found!!!";
                    return response;
                }

                if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".pdf";
                }

                string filePath = Path.Combine(uploadDirectory, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    response.ErrorMessage = "No file was found. Please contact driver";
                    return response;
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                string originalFileName = fileName;

                response.DownloadedFile = new FileDto
                {
                    FileName = originalFileName,
                    ContentType = "application/pdf",
                    Content = fileBytes
                };

                response.Message = "File downloaded successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error downloading file: {ex.Message}";
                return response;
            }
        }

        public async Task ActivateDriverByIdAsync(int driverId)
        {
            await this.driverDetailRepository.ActivateDriverAsync(driverId);
        }

        public async Task DeactivateDriverByIdAsync(int driverId)
        {
            await this.driverDetailRepository.DeactivateDriverAsync(driverId);
        }

        public async Task<DriverPositionResponse> UpsertDriverPositionAsync(DriverPositionRequest driverPositionRequest)
        {
            var response = new DriverPositionResponse();
            var driverPosition = new DriverPosition()
            {
                DriverId = driverPositionRequest.DriverId,
                Latitude = driverPositionRequest.Latitude,
                Longitude = driverPositionRequest.Longitude,
                ShipmentId = driverPositionRequest.ShipmentId,
                Waypoint = driverPositionRequest.Waypoint
            };

            var driverPos = await this.driverPositionRepository.GetDriverPositionAsync(driverPosition.DriverId);
            if (driverPos == null)
            {
                await this.driverPositionRepository.AddDriverPositionAsync(driverPosition);
                response.Message = "Driver Successfully created!";
            }
            else
            {
                await this.driverPositionRepository.UpdateDriverPositionAsync(driverPosition);
                response.Message = "Driver Successfully Updated!";
            }

            return response;
        }

        public async Task<DriverPositionResponse> GetDriverPositionAsync(int driverId)
        {
            var response = new DriverPositionResponse();
            var result = await this.driverPositionRepository.GetDriverPositionAsync(driverId);

            if(result == null)
            {
                response.ErrorMessage = "No positions found for driver";
            }
            else
            {
                response.Message = "Driver position found";
                response.DriverPosition = result;
            }

            return response;
        }
    }
}
