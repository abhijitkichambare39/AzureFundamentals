namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainers();
        Task<List<string>> GetAllContainersAndBlobs();
        Task<Dictionary<string, List<string>>> GetAllContainersAndBlobsDictonary();
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}
