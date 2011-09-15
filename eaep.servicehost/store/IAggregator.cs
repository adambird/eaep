using System;

namespace eaep.servicehost.store
{
    public interface IAggregator
    {
        int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices);
        int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy);
    }
}
