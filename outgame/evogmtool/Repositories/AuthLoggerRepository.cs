using Microsoft.Extensions.Logging;
using static evogmtool.Enums;

namespace evogmtool.Repositories
{
    public interface IAuthLoggerRepository
    {
        void Log(string account, AuthResult result);
    }

    public class AuthLoggerRepository : IAuthLoggerRepository
    {
        private readonly ILogger _authLogger;

        public AuthLoggerRepository(ILoggerFactory loggerFactory)
        {
            _authLogger = loggerFactory.CreateLogger("AuthLogger");
        }

        public void Log(string account, AuthResult result)
        {
            NLog.MappedDiagnosticsLogicalContext.Set("account", account);
            NLog.MappedDiagnosticsLogicalContext.Set("result", (int)result);
            _authLogger.LogInformation(string.Empty);
        }
    }
}
