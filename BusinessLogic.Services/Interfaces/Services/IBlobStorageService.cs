using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services.Interfaces.Services
{
    public interface IBlobStorageService
    {
        public Task UploadToBlobStorage(IFormFile file);
        public Task<string> GetBlob(string blobName);
        public Task DeleteBlob(string blobName);
    }
}
