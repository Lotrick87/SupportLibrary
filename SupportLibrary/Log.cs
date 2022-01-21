using System;
using System.Diagnostics;

namespace SupportLibrary
{
    public class Log
    {

        private static Action<TraceEventType, object, object[]> _log;

        public Log(Func<string, object, object> api)
        {
            _log = api("logger", "TraceEventType") as Action<TraceEventType, object, object[]>;
        }

        public static void Info(string textToWrite)
        {
            _log(TraceEventType.Information, textToWrite, null);
        }

        public static void Warning(string textToWrite)
        {
            _log(TraceEventType.Warning, textToWrite, null);
        }

        public static void Error(Exception exception, string textToWrite = null)
        {
            if (textToWrite != null)
            {
                _log(TraceEventType.Error, textToWrite, null);
            }
            _log(TraceEventType.Error, exception, null);
        }
    }
}
