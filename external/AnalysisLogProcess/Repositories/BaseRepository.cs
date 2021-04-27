using System;
using System.Globalization;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;

namespace AnalysisLogProcess.Repositories
{
    public abstract class BaseRepository<TLog>
    {
        protected BigQueryClient Client;

        protected string DatasetId { get; }

        public BaseRepository(BigQueryClient client, string datasetId)
        {
            Client = client;
            DatasetId = datasetId;
        }

        public abstract void Add(TLog log, ref int insertCount);

        public abstract Task InsertAsync();

        protected DateTime DateTimeParseToUtc(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString, null, DateTimeStyles.AdjustToUniversal);
        }
    }
}
