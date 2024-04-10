
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }



        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            await obj.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            await obj.DeleteIfExistsAsync();

        }

        public async Task<List<string>> GetAllContainers()
        {
            List<string> containerNames = new();
            await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync())
            {
                containerNames.Add(item.Name);
            }

            return containerNames;
        }

        public Task<List<string>> GetAllContainersAndBlobs()
        {
            throw new NotImplementedException();
        }
    }
}
