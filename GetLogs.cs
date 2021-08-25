using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace backend
{
	public static class GetRecentLogs
	{

		[FunctionName("GetRecentLogs")]
		public static async Task<IActionResult> getAll(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
			[Table("logs", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
           		ILogger log
		){
            		log.LogInformation("HTTP trigger function processed a request to GetRecentLogs.");
			int defaultNumOfEntries = 10;
			int numOfEntries;
			int.TryParse(req.Query["numOfEntries"], out numOfEntries);
			if ( numOfEntries <= 0 ){
				numOfEntries = defaultNumOfEntries; 
			}

			TableQuery<LogEntry> query = new TableQuery<LogEntry>().Take(numOfEntries);
        		//var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
        		//var data = segment.Results() .Select(LogExtensions.ToLogEntries);
			
			var data = cloudTable.ExecuteQuery(query);
			return new OkObjectResult(data);

		}
	}
}
