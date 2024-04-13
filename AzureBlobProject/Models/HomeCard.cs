using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Models
{
    public class HomeCard
    {
        List<BlobContainerItem> BlobContainerItemList { get; set; }
        List<BlobItem> BlobItemList { get; set; }

    }
}
