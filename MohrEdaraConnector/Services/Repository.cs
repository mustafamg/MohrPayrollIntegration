using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MohrEdaraConnector.Model;

namespace MohrEdaraConnector.Services
{
    public static class Repository
    {
        /// <summary>
        ///     Demonstrate the most efficient storage query - the point query - where both partition key and row key are
        ///     specified.
        /// </summary>
        /// <param name="partitionKey">Partition key - i.e., last name</param>
        /// <param name="rowKey">Row key - i.e., first name</param>
        /// <returns>A Task object</returns>
        public static async Task<T> RetrieveAsync<T>(string partitionKey, string rowKey) where T : AccountBaseTable
        {
            try
            {
                var table = BuildTableClient<T>();
                var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

                var result = await table.ExecuteAsync(retrieveOperation);
                if (result.Result is T entity)
                {
                    Console.WriteLine("\t{0}\t{1} retrieved", entity.PartitionKey, entity.RowKey);
                    return entity;
                }

                return null;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        /// <summary>
        ///     Demonstrate the most efficient storage query - the point query - where both partition key and row key are
        ///     specified.
        /// </summary>
        /// <param name="tenantId">Mohr Tenant Id</param>
        /// <returns>A Task object</returns>
        public static async Task<IEnumerable<T>> RetrieveAllAsync<T>(string tenantId) where T : AccountBaseTable
        {
            try
            {
                var table = BuildTableClient<T>();
                var tableQuery = new TableQuery<AccountBaseTable>().Where(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey", QueryComparisons.Equal, tenantId)
                    );
                TableContinuationToken continuationToken = null;//default(TableContinuationToken); ;
                var results = new List<T>();

                do
                {
                    // Retrieve a segment (up to 1,000 entities).
                    var tableQueryResult =
                        await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                    // Assign the new continuation token to tell the service where to
                    // continue on the next iteration (or null if it has reached the end).
                    continuationToken = tableQueryResult.ContinuationToken;

                    foreach (var entity in tableQueryResult.Results)
                    {
                        results.Add(entity as T);
                    }
                    // Loop until a null continuation token is received, indicating the end of the table.
                } while (continuationToken != null);


                return results;

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        /// <summary>
        ///     The Table Service supports two main types of insert operations.
        ///     1. Insert - insert a new entity. If an entity already exists with the same PK + RK an exception will be thrown.
        ///     2. Replace - replace an existing entity. Replace an existing entity with a new entity.
        ///     3. Insert or Replace - insert the entity if the entity does not exist, or if the entity exists, replace the
        ///     existing one.
        ///     4. Insert or Merge - insert the entity if the entity does not exist or, if the entity exists, merges the provided
        ///     entity properties with the already existing ones.
        /// </summary>
        /// <param name="entity">The entity to insert or merge</param>
        /// <returns>A Task object</returns>
        public static async Task<T> InsertOrMergeAsync<T>(T entity) where T : AccountBaseTable
        {
            if (entity == null) throw new ArgumentNullException("entity");

            try
            {
                var table = BuildTableClient<T>();
                // Create the InsertOrReplace table operation
                var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                var result = await table.ExecuteAsync(insertOrMergeOperation);
                var insertedCustomer = result.Result as T;

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        /// <summary>
        ///     Delete an entity
        /// </summary>
        /// <param name="deleteEntity">Entity to delete</param>
        /// <returns>A Task object</returns>
        public static async Task DeleteAsync<T>(T deleteEntity) where T : AccountBaseTable
        {
            try
            {
                var table = BuildTableClient<T>();
                if (deleteEntity == null) throw new ArgumentNullException("deleteEntity");

                var deleteOperation = TableOperation.Delete(deleteEntity);
                await table.ExecuteAsync(deleteOperation);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public static async Task CreateTableIfNotExist<T>() where T : AccountBaseTable
        {
            var table = BuildTableClient<T>();
            await table.CreateIfNotExistsAsync();
        }

        private static CloudTable BuildTableClient<T>() where T : AccountBaseTable
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(typeof(T).Name);
            return table;
        }
    }
}