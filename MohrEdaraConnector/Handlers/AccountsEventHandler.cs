using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using MohrEdaraConnector.Model;
using MohrEdaraConnector.Services;

namespace MohrEdaraConnector.Handlers
{
    public class AccountsEventHandler
    {
        private readonly TraceWriter _log;
        public AccountsEventHandler(TraceWriter log)
        {
            _log = log;

        }

        public async Task Handle(EventInfo args)
        {
            var proxy = new EdaraProxy();;
            _log.Info($"{this.GetType().Name} triggered.");
            // Todo: Create local accural account
            if (args.ActionType != "Insert")
            {
                _log.Error("Insert account only supported");
                return;
            }
            switch (args.EntityType)
            {
                case "AccrualAccount":
                    var accrualAccount = proxy.GetAccrualAccount(args.EntityId);
                    await Repository.InsertOrMergeAsync(accrualAccount);
                    break;
                case "ExpensesAccount":
                    var expensesAccount = proxy.GetExpensesAccount(args.EntityId);
                    await Repository.InsertOrMergeAsync(expensesAccount);
                    break;
                case "CostCenter":
                    var costCenter = proxy.GetCostCenter(args.EntityId);
                    await Repository.InsertOrMergeAsync(costCenter);
                    break;
                default:
                    _log.Error($"type sent {args.EntityType} is not supported");
                    break;

            }
        }
    }
}
