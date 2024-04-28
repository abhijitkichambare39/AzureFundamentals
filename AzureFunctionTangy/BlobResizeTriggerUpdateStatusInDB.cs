using System;
using System.IO;
using System.Linq;
using AzureFunctionTangy.Data;
using AzureFunctionTangy.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTangy
{
    public class BlobResizeTriggerUpdateStatusInDB
    {
        private readonly AzureTangyDbContext _db;

        public BlobResizeTriggerUpdateStatusInDB(AzureTangyDbContext db)
        {
            _db = db;
        }


        [FunctionName("BlobResizeTriggerUpdateStatusInDB")]
        public void Run([BlobTrigger("functionsalesrep-sm/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob
            , string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var fileName = Path.GetFileNameWithoutExtension(name);
            SalesRequest salesRequestfromDb = _db.SalesRequests.FirstOrDefault(x => x.Id == fileName);
            if (salesRequestfromDb != null)
            {
                salesRequestfromDb.Status = "Image Processed";
                _db.SalesRequests.Update(salesRequestfromDb);
                _db.SaveChanges();  
            }


        }
    }
}
