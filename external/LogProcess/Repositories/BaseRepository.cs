using System;
using System.Globalization;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace LogProcess.Repositories
{
    public abstract class BaseRepository<TLog>
    {
        protected DataConnection DataConnection { get; }

        public BaseRepository(DataConnection dataConnection)
        {
            DataConnection = dataConnection;
        }

        public abstract void Add(TLog log, ref int insertCount);

        public abstract Task InsertAsync();

        protected DateTime DateTimeParseToUtc(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString, null, DateTimeStyles.AdjustToUniversal);
        }

        protected static BulkCopyOptions GetBulkCopyOptions()
        {
            return new BulkCopyOptions
            {
                // todo: options.BatchSize 検討
            };
        }
    }
}
