
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace backend
{

    public static class AddLog
    {
        [FunctionName("AddLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Table("logs", Connection = "AzureWebJobsStorage")] IAsyncCollector<LogEntry> logsTableCollector,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function AddLog processing a request.");

            string text = req.Query["text"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            text = text ?? data?.text;

            var logEntry = new Log
            {
                text = text
            };

            try
            {
                await logsTableCollector.AddAsync(logEntry.ToTable());
            }
            catch (System.Exception)
            {
                throw;
            }
            return new OkObjectResult(logEntry);

        }
    }
}
