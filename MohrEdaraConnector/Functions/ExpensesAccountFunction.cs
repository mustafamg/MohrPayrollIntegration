using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MohrEdaraConnector.Model;
using MohrEdaraConnector.Services;

namespace MohrEdaraConnector.Functions
{
    public static class ExpensesAccountFunction
    {
        [FunctionName("ExpensesAccount")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string tenantId = req.Query["tenantId"];

            var ret = Repository.RetrieveAllAsync<ExpensesAccount>(tenantId).GetAwaiter().GetResult();
            return ret != null
                ? (ActionResult)new OkObjectResult(ret)
                : new BadRequestObjectResult("No data or incorrect tenant Id");
        }
    }
}
