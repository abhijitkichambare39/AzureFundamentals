using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IContainerService _containerService;

        public HomeController(ILogger<HomeController> logger, IContainerService containerService)
        {
            _logger = logger;
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _containerService.GetAllContainersAndBlobsDictonary());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
