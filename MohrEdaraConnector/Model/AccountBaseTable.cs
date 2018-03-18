using Microsoft.WindowsAzure.Storage.Table;

namespace MohrEdaraConnector.Model
{
    public class AccountBaseTable:TableEntity
    {
        public int MohrTenantId { get; set; }
        public string EdaraToken { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }

        #region Constructors

        public AccountBaseTable(string mohrTenantId, string value)
        {
            PartitionKey = $"{mohrTenantId}";
            RowKey = $"{value}";//-{id}
        }

        public AccountBaseTable()
        {
        }

        #endregion
    }
}