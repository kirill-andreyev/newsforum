using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessLogic.Services.Interfaces.Services;
using Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Services.Implementations.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var blobServiceClient =
                new BlobServiceClient(configuration.GetConnectionString(ConstantsStrings.BlobConnectionPropertyName));
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(ConstantsStrings.BlobContainerClient);
            _blobContainerClient.CreateIfNotExists();
            _blobContainerClient.SetAccessPolicy(PublicAccessType.BlobContainer);
        }

        public async Task UploadToBlobStorage(IFormFile file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(file.FileName);
            if (await blobClient.ExistsAsync())
            {
                throw new Exception("File already exist. Select another one or rename current file");
            }

            await blobClient.UploadAsync(file.OpenReadStream());
        }

        public async Task<string> GetBlob(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var blobUrl = blobClient.Uri.AbsoluteUri;
            return blobUrl;
        }

        public async Task DeleteBlob(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteAsync();
        }
    }
}
