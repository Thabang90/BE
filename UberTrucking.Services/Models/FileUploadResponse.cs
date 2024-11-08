using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class FileUploadResponse
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }

        public FileDto DownloadedFile { get; set; }
    }
}
