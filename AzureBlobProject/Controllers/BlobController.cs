using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using AzureBlobProject.Models;
using Blob = AzureBlobProject.Models.Blob;

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;


        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }


        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobsObj = await _blobService.GetAllBlobs(containerName);
            return View(blobsObj);
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName,IFormFile file ,Blob blob)
        {
            if (file == null || file.Length < 1) return View();

            //name.png
            //name_guid.png for same filename restriction
            var filename =Path.GetFileNameWithoutExtension(file.FileName)+ "_"+ Guid.NewGuid() + Path.GetExtension(file.FileName);
            var result = await _blobService.UploadBlob(filename, file, containerName,blob);

            if (result)
            {
                return RedirectToAction("Index", "Container");
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ViewFile(string name ,string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }

        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "Container");
        }
    }
}
