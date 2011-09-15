using System;

namespace eaep.servicehost.store
{
    public interface IEAEPMonitorStore : IDisposable
    {
        void Close();
        EAEPMessages GetMessages(string query);
        EAEPMessages GetMessages(DateTime since, string query);
        EAEPMessages GetMessages(DateTime from, DateTime to, string query);
        void PushMessage(EAEPMessage message);
        void PushMessages(EAEPMessages messages);
        string[] Distinct(string field, DateTime from, DateTime to, string query);
        CountResult[] Count(DateTime from, DateTime to, int timeSlices, string query);
        CountResult[] Count(DateTime from, DateTime to, int timeSlices, string field, string query);

    }
}
