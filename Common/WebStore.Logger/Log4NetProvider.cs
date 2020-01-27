using Microsoft.Extensions.Logging;
using System.Xml;
using System.Collections.Concurrent;

namespace WebStore.Logger
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;

        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers 
            = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;

        public ILogger CreateLogger(string CategoryName)
        {
            return _Loggers.GetOrAdd(CategoryName, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_ConfigurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose() => _Loggers.Clear();
    }
}
