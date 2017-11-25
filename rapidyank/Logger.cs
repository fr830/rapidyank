using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace rapidyank
{
    public static class Logger
    {
        public static void Trace(string message, string loggerName = null)
        {
#if TRACE
            Log(LogLevel.Trace, message, null, loggerName);
#endif
        }
        public static void Trace(Exception exception, string message = null, string loggerName = null)
        {
#if TRACE
            Log(LogLevel.Trace, message ?? exception.Message, exception, loggerName);
#endif
        }
        public static void Debug(string message, string loggerName = null)
        {
#if DEBUG
            Log(LogLevel.Debug, message, null, loggerName);
#endif
        }
        public static void Debug(Exception exception, string message = null, string loggerName = null)
        {
#if DEBUG
            Log(LogLevel.Debug, message ?? exception.Message, exception, loggerName);
#endif
        }
        public static void Info(string message, string loggerName = null) { Log(LogLevel.Info, message, null, loggerName); }
        public static void Info(Exception exception, string message = null, string loggerName = null) { Log(LogLevel.Info, message ?? exception.Message, exception, loggerName); }
        public static void Warn(string message, string loggerName = null) { Log(LogLevel.Warn, message, null, loggerName); }
        public static void Warn(Exception exception, string message = null, string loggerName = null) { Log(LogLevel.Warn, message ?? exception.Message, exception, loggerName); }
        public static void Error(string message, string loggerName = null) { Log(LogLevel.Error, message, null, loggerName); }
        public static void Error(Exception exception, string message = null, string loggerName = null) { Log(LogLevel.Error, message ?? exception.Message, exception, loggerName); }
        public static void Fatal(string message, string loggerName = null) { Log(LogLevel.Fatal, message, null, loggerName); }
        public static void Fatal(Exception exception, string message = null, string loggerName = null) { Log(LogLevel.Fatal, message ?? exception.Message, exception, loggerName); }

        private static void Log(LogLevel level, string message, Exception exception = null, string loggerName = null)
        {
            NLog.Logger logger;
            if (loggerName != null)
                logger = LogManager.GetLogger(loggerName);
            else
            {
                //var type = new StackFrame(skipFrames: 2).GetMethod().DeclaringType;
                logger = LogManager.GetLogger("default");
            }

            if (exception != null)
                logger.Log(level, exception, message);
            else
                logger.Log(level, message);
        }
    }
}
