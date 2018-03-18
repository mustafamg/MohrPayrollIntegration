using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using MohrEdaraConnector.Model;
using Newtonsoft.Json;

namespace MohrEdaraConnector.Functions
{
    public static class PayRunEventHandler
    {
        [FunctionName("PayRunEventHandler")]
        public static void Run(
            [QueueTrigger("PayRun", Connection = "StorageConnectionString")]
            string queueItem,
            TraceWriter log)
        {
            log.Info($"A PayRun processing triggered: {queueItem}");
            var payRun = JsonConvert.DeserializeObject<Salary>(queueItem);
            //Add Journal Here Using Edara Proxy
        }
    }
}
