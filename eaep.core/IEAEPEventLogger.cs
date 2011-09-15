using System;

namespace eaep
{
    public interface IEAEPEventLogger
    {
        void LogEvent(string eventName, params EventParameter[] eventParameters);
        void LogEvent(DateTime timestamp, string eventName, params EventParameter[] parameters);
    }
}