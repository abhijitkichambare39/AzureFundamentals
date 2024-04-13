
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;


        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }



        #region-Crud
        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = obj.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType,
            };


            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
            if (result != null)
            {
                return true;
            }

            return false;

        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = obj.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();

        }
        #endregion



        #region-GET
        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = obj.GetBlobsAsync();

            var blobList = new List<string>();
            await foreach (var item in blobs)
            {
                blobList.Add(item.Name);
            }

            return blobList;

        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = obj.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }
        #endregion


    }
}
