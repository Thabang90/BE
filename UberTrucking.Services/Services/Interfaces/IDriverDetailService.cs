using Microsoft.AspNetCore.Http;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IDriverDetailService
    {
        Task<DriverDetailResponse> CreateDriverDetailAsync(DriverDetailRequest driverDetailRequest);
        Task<DriverDetailResponse> GetDriverDetailsById(int driverId);

        Task<DriverDetailResponse> GetAllDriversAsync();

        Task<DriverDetailResponse> GetAvailableDriversAsync();
        Task<FileUploadResponse> UploadPdfDocumentAsync(FileUploadRequest request);
        Task<FileUploadResponse> DownloadPdfAsync(string fileName);

        Task ActivateDriverByIdAsync(int driverId);
        Task DeactivateDriverByIdAsync(int driverId);

        Task<DriverPositionResponse> GetDriverPositionAsync(int driverId);
        Task<DriverPositionResponse> UpsertDriverPositionAsync(DriverPositionRequest driverPositionRequest);
    }
}
