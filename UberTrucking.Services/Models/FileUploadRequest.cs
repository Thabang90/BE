using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class FileUploadRequest
    {
        public string DriverId { get; set; }
        public IFormFile File { get; set; }
    }
}
