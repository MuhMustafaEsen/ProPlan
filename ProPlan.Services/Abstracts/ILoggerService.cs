using ProPlan.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ILoggerService
    {
        void Log(LogLevel level, string message);
        void LogError(string message,Exception ex = null);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
    }
}
