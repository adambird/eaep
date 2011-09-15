using System;
using System.Collections.Generic;

namespace eaep.servicehost.store
{
    public class Aggregator : IAggregator
    {
        protected IEAEPMonitorStore store;

        public Aggregator(IEAEPMonitorStore store)
        {
            this.store = store;
        }

        public int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices)
        {
            return Count(query, rangeFrom, rangeTo, timeSlices, null);
        }

        public int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy)
        {
            int[] result = new int[timeSlices];

            TimeSpan rangeDuration = rangeTo.Subtract(rangeFrom);
            int timeSliceDuration = (int)rangeDuration.TotalMilliseconds / timeSlices;

            for (int timeSlice = 0; timeSlice < timeSlices; timeSlice++)
            {
                EAEPMessages messages = store.GetMessages(
                    rangeFrom.AddMilliseconds(timeSlice * timeSliceDuration),
                    rangeFrom.AddMilliseconds((timeSlice + 1) * timeSliceDuration),
                    query);

                if (groupBy != null)
                {
                    HashSet<string> groups = new HashSet<string>();

                    foreach (EAEPMessage message in messages)
                    {
                        if (message.ContainsParameter(groupBy) && !groups.Contains(message[groupBy]))
                        {
                            groups.Add(message[groupBy]);
                        }
                    }

                    result[timeSlice] = groups.Count;
                }
                else
                {
                    result[timeSlice] = messages.Count;
                }
            }
            return result;
        }

        
    }
}
