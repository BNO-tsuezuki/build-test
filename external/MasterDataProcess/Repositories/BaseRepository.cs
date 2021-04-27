using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models.Master;

namespace MasterDataProcess.Repositories
{
    public abstract class BaseRepository
    {
        protected BigQueryClient Client;

        protected string DatasetId { get; }

        protected string TableName { get; }

        protected MasterData MasterData { get; }

        private QueryOptions CommonQueryOptions = new QueryOptions
        {
            Priority = QueryPriority.Interactive,
            UseQueryCache = false,
        };

        public BaseRepository(BigQueryClient client, string datasetId, string tableName, MasterData masterData)
        {
            Client = client;
            DatasetId = datasetId;
            TableName = tableName;
            MasterData = masterData;
        }

        public async Task TruncateAndInsertAsync()
        {
            var sw = Stopwatch.StartNew();

            var countToInsert = 0;

            var query = BuildTruncateQuery() + BuildInsertQuery(ref countToInsert);

            if (countToInsert == 0)
            {
                // todo: error message
                LambdaLogger.Log($"[ERROR] [{TableName}] countToInsert: 0");
            }

            await Client.ExecuteQueryAsync(query, null, CommonQueryOptions);

            var insertedCount = await GetInsertedCount();

            if (countToInsert != insertedCount)
            {
                // todo: error message
                LambdaLogger.Log($"[ERROR] [{TableName}] countToInsert: {countToInsert}, insertedCount: {insertedCount}");
            }

            LambdaLogger.Log($"[INFO ] [{TableName}] {sw.Elapsed.TotalMilliseconds:N0} msec. {countToInsert} rows inserted.");
        }

        protected string BuildTruncateQuery()
        {
            return $"TRUNCATE TABLE {DatasetId}.{TableName};";
        }

        protected abstract string BuildInsertQuery(ref int count);

        protected string EscapeLiteral(string text)
        {
            return text.Replace(@"\", @"\\").Replace(@"'", @"\'");
        }

        protected DateTime DateTimeParseToUtc(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString, null, DateTimeStyles.AdjustToUniversal);
        }

        private async Task<long> GetInsertedCount()
        {
            var query = $"SELECT COUNT(*) FROM {DatasetId}.{TableName};";

            var result = await Client.ExecuteQueryAsync(query, null, CommonQueryOptions);

            var c = result.FirstOrDefault()[0].ToString();

            return int.Parse(c);
        }
    }
}
