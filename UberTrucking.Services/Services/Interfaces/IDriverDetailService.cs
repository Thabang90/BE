using Microsoft.AspNetCore.Http;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IDriverDetailService
    {
        Task<DriverDetailResponse> CreateDriverDetailAsync(DriverDetailRequest driverDetailRequest);
        Task<DriverDetailResponse> GetDriverDetailsById(int driverId);
        Task<DriverDetailResponse> GetAvailableDriversAsync();
        Task<FileUploadResponse> UploadPdfDocumentAsync(IFormFile file);
        Task<FileUploadResponse> DownloadPdfAsync(string fileName);
    }
}
