using System;
using System.Net.Http;
using System.Net.Http.Headers;
using MohrEdaraConnector.Model;

namespace MohrEdaraConnector.Services
{


    public class EdaraProxy
    {
        private readonly HttpClient _client = new HttpClient();
        public EdaraProxy()
        {
            // Update port # in the following line.
            _client.BaseAddress = new Uri("https://api.getedara.com/v/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // todo: Add security tocken here
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your Oauth token");

        }

        public int InsertJournal(string journal)
        {
            throw new NotImplementedException();
        }

        public AccountBaseTable GetCostCenter(string costCenterId)
        {
            throw new NotImplementedException();
        }

        public AccountBaseTable GetExpensesAccount(string expensesId)
        {
            throw new NotImplementedException();
        }

        public AccountBaseTable GetAccrualAccount(string accrualId)
        {
            throw new NotImplementedException();
        }
    }
}
