using System;
using eaep.servicehost.store;

namespace eaep.servicehost.test.http
{
    class MockAggregator : IAggregator
    {
        public int[] PrimedResult { get; set; }
        public string Query { get; set; }
        public DateTime RangeFrom { get; set; }
        public DateTime RangeTo { get; set; }
        public int TimeSlices { get; set; }
        public string GroupBy { get; set; }

        #region IAggregator Members

        public int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices)
        {
            return Count(query, rangeFrom, rangeTo, timeSlices, null);
        }

        public int[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy)
        {
            this.Query = query;
            this.RangeFrom = rangeFrom;
            this.RangeTo = rangeTo;
            this.TimeSlices = timeSlices;
            this.GroupBy = groupBy;

            return PrimedResult;
        }

        #endregion
    }
}
