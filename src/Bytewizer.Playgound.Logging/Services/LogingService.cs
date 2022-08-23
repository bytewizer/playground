//using System;

//using Bytewizer.TinyCLR.Hosting;
//using Bytewizer.TinyCLR.Logging;

//namespace Bytewizer.Playground.Logging
//{
//    public class LoggingService : SchedulerService
//    {
//        private readonly ILogger _logger;

//        public LoggingService(ILoggerFactory loggerFactory)
//           : base(TimeSpan.FromSeconds(10))
//        {
//            _logger = loggerFactory.CreateLogger(nameof(Program));

//            _logger.Log(LogLevel.Critical, new EventId(10, "Id Name"), "logging levels enabled:", null);
//            _logger.LogTrace("Trace");
//            _logger.LogDebug("Debug");
//            _logger.LogInformation("Information");
//            _logger.LogWarning("Warning");
//            _logger.LogError("Error");
//            _logger.LogCritical("Critical");
//        }

//        protected override void ExecuteAsync()
//        {
//            _logger.LogInformation("Logging message at {0}", DateTime.Now);
//        }
//    }
//}
