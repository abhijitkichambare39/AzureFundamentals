
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureBlobProject.Models;
using System.Net;

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
            //string sasContainerSignature = "";

            // if SAS-TOKEN at STORAGE-ACCOUNT LEVEL 
            // go to azureportal --> storage-Account --> shared-Access-Signatures --> Allowed Resources tick all checkboxes --->
            // Generate SAS  & Conn String --> Copy & Paste SAS-Conn-String in appsettings.JSON

            #region SAS-Token CONTAINER-LEVEL
            //if (objBlobContainer.CanGenerateSasUri)
            //{
            //    BlobSasBuilder sasBuilder = new()
            //    {
            //        BlobContainerName = objBlobContainer.Name,
            //        //BlobName = bobClient.Name,
            //        Resource = "b",
            //        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            //    };
            //    sasBuilder.SetPermissions(BlobSasPermissions.Read);// ) || BlobSasPermissions.Write);
            //    //blobIndividual.Uri = bobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
            //    sasContainerSignature = objBlobContainer.GenerateSasUri(sasBuilder).AbsoluteUri.Split("?")[1].ToString();
            //}
            #endregion

            await foreach (var item in blobs)
            {
                var bobClient = objBlobContainer.GetBlobClient(item.Name);
                Blob blobIndividual = new()
                {
                    // if container level added then attach ? + sassignature ---- else bobClient.Uri.AbsoluteUri
                    Uri = bobClient.Uri.AbsoluteUri // + "?" + sasContainerSignature


                };

                #region SAS-Token BLOB-Level . We can also do at CONTAINER-LEVEL
                //if (bobClient.CanGenerateSasUri)
                //{
                //    BlobSasBuilder sasBuilder = new()
                //    {
                //        BlobContainerName = bobClient.GetParentBlobContainerClient().Name,
                //        BlobName = bobClient.Name,
                //        Resource = "b",
                //        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                //    };
                //    sasBuilder.SetPermissions(BlobSasPermissions.Read);// ) || BlobSasPermissions.Write);
                //    blobIndividual.Uri = bobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
                //}
                #endregion

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
