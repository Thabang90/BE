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
        private readonly IConfiguration config;

        public DriverDetailService(IDriverDetailRepository driverDetailRepository, IConfiguration config)
        {
            this.driverDetailRepository = driverDetailRepository;
            this.config = config;
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

        public async Task<FileUploadResponse> UploadPdfDocumentAsync(IFormFile file)
        {
            var response = new FileUploadResponse();
            var uploadDirectory = this.config["FileUpload:UploadDirectory"];

            if(!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                response.ErrorMessage = "Only PDF files are allowed!";
                return response;
            }

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            string uniqueFileName = $"{Guid.NewGuid().ToString().Substring(0, 4)}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
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
                var uploadDirectory = this.config["FileUpload:UploadDirectory"];

                if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".pdf";
                }

                string filePath = Path.Combine(uploadDirectory, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    response.ErrorMessage = $"File {fileName} not found";
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                string originalFileName = fileName.Substring(fileName.IndexOf('_') + 1);

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
    }

}
