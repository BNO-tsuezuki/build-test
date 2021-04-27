using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using evolib.Log;

namespace evolib.Databases
{
    public class SqlLoggerProvider : ILoggerProvider
    {
		public void Dispose() { }

		public ILogger CreateLogger(string categoryName)
		{
			if (categoryName != DbLoggerCategory.Database.Command.Name)
			{
				return NullLogger.Instance;
			}

			return new SqlLogger();
		}


		class SqlLogger : ILogger
		{
			public IDisposable BeginScope<TState>(TState state) => null;
			
			public bool IsEnabled(LogLevel logLevel) => true;

			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
										Exception exception, Func<TState, Exception, string> formatter)
			{
				Logger.Logging(
					new LogObj().AddChild(new LogModels.DataBase
					{
						State = formatter(state, exception),
					})
				);
			}
		}
	}
}
