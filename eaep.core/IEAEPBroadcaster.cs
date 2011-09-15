using System;

namespace eaep
{
    [Obsolete("Use IEAEPEventLogger interface instead", true)]
    public interface IEAEPBroadcaster
    {
        string Application { get; set; }
        string Host { get; set; }
        void LogEvent(string eventName, params EventParameter[] parameters);
        void LogEvent(DateTime timestamp, string eventName, params EventParameter[] parameters);
    }
}