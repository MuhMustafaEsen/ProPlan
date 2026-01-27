using NLog;
using ProPlan.Services.Abstracts;
using ProPlan.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Contracts
{
    public class LoggerManager : ILoggerService
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void Log(Enums.LogLevel level, string message)
        {
            switch (level)
            {
                case Enums.LogLevel.Debug:
                    logger.Debug(message);
                    break;
                case Enums.LogLevel.Info:
                    logger.Info(message);
                    break;
                case Enums.LogLevel.Warn:
                    logger.Warn(message);
                    break;
                case Enums.LogLevel.Error:
                    logger.Error(message);
                    break;
                default:
                    logger.Info(message); // Varsayılan olarak Info seviyesinde log yapabiliriz.
                    break;
            }
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex == null)
            {
                logger.Error(message);
            }
            else
            {
                logger.Error(ex, message);
            }
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
