
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using MohrEdaraConnector.Handlers;
using MohrEdaraConnector.Model;
using Newtonsoft.Json;

namespace MohrEdaraConnector.Functions
{
    public static class EdaraEventHandler
    {
        [FunctionName("EdaraEventHandler")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            TraceWriter log)
        {
            log.Info("EdaraEventHandler function processed a request.");

            var requestBody = new StreamReader(req.Body).ReadToEnd();
            var eventInfo = JsonConvert.DeserializeObject<EventInfo>(requestBody);

            new AccountsEventHandler(log).Handle(eventInfo).GetAwaiter().GetResult();

            return new OkResult();
        }
    }
}
