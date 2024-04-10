namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainers();
        Task<List<string>> GetAllContainersAndBlobs();
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}
