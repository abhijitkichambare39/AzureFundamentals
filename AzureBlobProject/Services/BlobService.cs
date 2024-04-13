
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Models;

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
        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient obj = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = obj.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType,
            };

            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("title", blob.Title);
            metadata["comment"] = blob.Comment;

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata);

            #region- Remove metadata by key-&-value
            //IDictionary<string, string> removeMetadata = new Dictionary<string, string>(); or another way
            //metadata.Remove("title");
            //await blobClient.SetMetadataAsync(metadata);
            #endregion

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

        public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient objBlobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = objBlobContainer.GetBlobsAsync();

            var blobList = new List<Blob>();

            await foreach (var item in blobs)
            {
                var bobClient = objBlobContainer.GetBlobClient(item.Name);
                Blob blobIndividual = new()
                {
                    Uri = bobClient.Uri.AbsoluteUri
                };
                BlobProperties properties = await bobClient.GetPropertiesAsync();
                if (properties.Metadata.ContainsKey("title"))
                {
                    blobIndividual.Title = properties.Metadata["title"];
                }
                if (properties.Metadata.ContainsKey("comment"))
                {
                    blobIndividual.Comment = properties.Metadata["comment"];
                }
                blobList.Add(blobIndividual);
            }
            return blobList;
        }
        #endregion


    }
}
