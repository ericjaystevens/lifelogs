using Xunit;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;

namespace backend.UnitTests.Services
{
    public class PrimeService_IsPrimeShould
    {
        [Fact]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            var primeService = new PrimeService();
            bool result = primeService.IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }
    }
    public class Backend_NoError
    {
        [Fact]
        public void Backend_ReturnsOk(){
            var ok = new OkResult();
            var req = GenerateHttpRequest();

            var mockLogger = new Mock<ILogger>();
            var uri = new Uri("http://127.0.0.1:10000/devstoreaccount1/logs");
            var ct = new MockCloudTable(uri);

            var returnResult = GetRecentLogs.getAll(req, ct, mockLogger.Object).Result;
            var okObjectResult = returnResult as OkObjectResult;

            Assert.Equal(200, okObjectResult.StatusCode);

        }
        private DefaultHttpRequest GenerateHttpRequest()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            return request;
        }
        
    }

    /// <summary>
    /// Mock class for CloudTable object
    /// </summary>
    public class MockCloudTable : CloudTable
    {

        public MockCloudTable(Uri tableAddress) : base(tableAddress)
        { }

        public MockCloudTable(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        { }

        public MockCloudTable(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        { }

        public async override Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                Result = new LogEntry() { text = "foo" },
                HttpStatusCode = 200
            });
        }
    }
}