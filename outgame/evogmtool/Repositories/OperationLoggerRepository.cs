using System;
using Microsoft.Extensions.Logging;

namespace evogmtool.Repositories
{
    public interface IOperationLoggerRepository
    {
        void Info(int? userId, int statusCode, string responseBody);
        void Error(int? userId, int statusCode, Exception exception);
    }

    public class OperationLoggerRepository : IOperationLoggerRepository
    {
        private readonly ILogger _operationLogger;

        public OperationLoggerRepository(ILoggerFactory loggerFactory)
        {
            _operationLogger = loggerFactory.CreateLogger("OperationLogger");
        }

        public void Info(int? userId, int statusCode, string responseBody)
        {
            NLog.MappedDiagnosticsLogicalContext.Set("userId", userId);
            NLog.MappedDiagnosticsLogicalContext.Set("statusCode", statusCode);
            NLog.MappedDiagnosticsLogicalContext.Set("responseBody", responseBody);
            _operationLogger.LogInformation(string.Empty);
        }

        public void Error(int? userId, int statusCode, Exception exception)
        {
            NLog.MappedDiagnosticsLogicalContext.Set("userId", userId);
            NLog.MappedDiagnosticsLogicalContext.Set("statusCode", statusCode);
            NLog.MappedDiagnosticsLogicalContext.Set("exception", exception.ToString());
            _operationLogger.LogInformation(string.Empty);
        }
    }
}
