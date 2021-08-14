
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
    public class LogEntry : TableEntity
    {
        public DateTime createdTime { get; set; }

        public string text { get; set; }
    }
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("logs", Connection = "AzureWebJobsStorage")] IAsyncCollector<LogEntry> logsTableCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var logEntry = new Log
            {
                text = name
            };

            await logsTableCollector.AddAsync(logEntry.ToTable());


            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name} Let's do this!";

            return new OkObjectResult(responseMessage);
        }
    }
}
