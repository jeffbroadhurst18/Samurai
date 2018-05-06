using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamuraiAppCore.Data
{
	public class MyLoggerProvider : ILoggerProvider
		{
		public ILogger CreateLogger(string categoryName)
		{
			return new MyLogger();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private class MyLogger : ILogger
		{
			public IDisposable BeginScope<TState>(TState state)
			{
				return null;
			}

			public bool IsEnabled(LogLevel logLevel)
			{
				return true;
			}

			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
			{
				//File.AppendAllText(@"C:\temp\log.txt", formatter(state, exception));
				
					Console.WriteLine(formatter(state, exception));
					Console.WriteLine();
				
			}
		}
	}

	
}
