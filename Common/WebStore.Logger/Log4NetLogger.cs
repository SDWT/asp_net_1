using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Xml;
using log4net;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string CategoryName, XmlElement Configuration)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(logger_repository.Name, CategoryName);
            log4net.Config.XmlConfigurator.Configure(logger_repository, Configuration);
        }

        public bool IsEnabled(LogLevel Level)
        {
            switch (Level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;

                case LogLevel.Information:
                    return _Log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;

                case LogLevel.Error:
                    return _Log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;

                case LogLevel.None:
                    return false;

                default: throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel)) return;

            var log_message = formatter(state, exception);

            if (string.IsNullOrEmpty(log_message) && exception is null) return;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_message);
                    break;

                case LogLevel.Information:
                    _Log.Info(log_message);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(log_message);
                    break;
                case LogLevel.Error:
                    _Log.Error(log_message);
                    break;
                case LogLevel.Critical:
                    _Log.Fatal(log_message);
                    break;

                case LogLevel.None:
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;

    }
}
