
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Models;
using System.Text;
using System.Web;

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

        public async Task<List<string>> GetAllContainersAndBlobs()
        {
            List<string> containerAndBlobNames = new();
            containerAndBlobNames.Add("Account Name : " + _blobServiceClient.AccountName);
            containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
            await foreach (BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containerAndBlobNames.Add("--" + blobContainerItem.Name);
                BlobContainerClient _blobContainer =
                      _blobServiceClient.GetBlobContainerClient(blobContainerItem.Name);
                await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    //get metadata
                    var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string blobToAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                    }

                    containerAndBlobNames.Add("------" + blobToAdd);
                }
                containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");

            }
            return containerAndBlobNames;
        }

        public async Task<Dictionary<string, List<string>>> GetAllContainersAndBlobsDictonary()
        {
            Dictionary<string, List<string>> containerBlobMap = new Dictionary<string, List<string>>();

            await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync())
            {
                List<string> blobs = new List<string>();
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(item.Name);

                await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
                {
                    blobs.Add(blobItem.Name);
                }

                containerBlobMap.Add(item.Name, blobs);
            }

            return containerBlobMap;
        }
    }
}
