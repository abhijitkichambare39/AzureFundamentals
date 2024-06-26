﻿using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;

        public HomeController(ILogger<HomeController> logger, IContainerService containerService,IBlobService blobService)
        {
            _logger = logger;
            _containerService = containerService;
            _blobService = blobService;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _containerService.GetAllContainersAndBlobsDictonary());
        }

        public async Task<IActionResult> Images()
        {
            List<Blob> a = await _blobService.GetAllBlobsWithUri("private-container-images");
            List<Blob> b = await _blobService.GetAllBlobsWithUri("public-container-images");

            List<Blob> res= new List<Blob>();
            res.AddRange(a);
            res.AddRange(b);
            return View(res);
            //return View(await _blobService.GetAllBlobsWithUri("private-container-images"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
