using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eaep.servicehost.store;

namespace eaep.servicehost.test.store
{
    
    
    /// <summary>
    ///This is a test class for LuceneAggregatorTest and is intended
    ///to contain all LuceneAggregatorTest Unit Tests
    ///</summary>
    [TestClass]
    [Ignore] // requires sql
    public class AggregatorTest
    {

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod]
        public void CountTest()
        {
            // Arrange
            var expected = new[] { 1, 2, 0, 1 };

            const string app1 = "app1";
			const string app2 = "app2";

            var messages = new EAEPMessages
                           {
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 5, 0), "host1", app1, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 10, 0), "host1", app2, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 17, 0), "host1", app1, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 18, 0), "host1", app2, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 22, 0), "host1", app1, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 36, 0), "host1", app2, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 51, 0), "host1", app1, "purchase"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 55, 0), "host1", app2, "purchase")
                           };

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                foreach (var message in messages)
                {
                    monitorStore.PushMessage(message);
                }

                var target = new Aggregator(monitorStore);

            // Act
                var actual = target.Count(
                    string.Format("{0}:{1} AND {2}:{3}", EAEPMessage.FIELD_APPLICATION, app1, EAEPMessage.FIELD_EVENT, "purchase"), 
                    new DateTime(2009, 9, 17, 12, 0, 0), 
                    new DateTime(2009, 9, 17, 13, 0, 0), 
                    4);

            // Assert
                Assert.AreEqual(expected.Length, actual.Length);

                for (var i = 0; i < expected.Length; i++)
                {
                    Assert.AreEqual(expected[i], actual[i]);
                }

            }
        }
        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod]
        public void CountUniqueTest()
        {
            // Arrange
            var expected = new[] { 1, 2, 1, 1 };

            const string user1 = "user1";
            const string user2 = "user2";

            var messages = new EAEPMessages
                           {
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 5, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 10, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 17, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 18, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 22, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 36, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 51, 0), "host1", "webapp", "PageLoad"),
                               new EAEPMessage(new DateTime(2009, 9, 17, 12, 55, 0), "host1", "webapp", "PageLoad")
                           };

            messages[0][EAEPMessage.PARAM_USER] = user1;
            messages[1][EAEPMessage.PARAM_USER] = user1;
            messages[2][EAEPMessage.PARAM_USER] = user1;
            messages[3][EAEPMessage.PARAM_USER] = user2;
            messages[4][EAEPMessage.PARAM_USER] = user2;
            messages[5][EAEPMessage.PARAM_USER] = user1;
            messages[6][EAEPMessage.PARAM_USER] = user2;
            messages[7][EAEPMessage.PARAM_USER] = user2;

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                foreach (var message in messages)
                {
                    monitorStore.PushMessage(message);
                }

                var target = new Aggregator(monitorStore);

                // Act
                var actual = target.Count(
                    string.Format("{0}:{1} AND {2}:{3}", EAEPMessage.FIELD_APPLICATION, "webapp", EAEPMessage.FIELD_HOST, "host1"),
                    new DateTime(2009, 9, 17, 12, 0, 0),
                    new DateTime(2009, 9, 17, 13, 0, 0),
                    4,
                    EAEPMessage.PARAM_USER);

                // Assert
                Assert.AreEqual(expected.Length, actual.Length);

                for (var i = 0; i < expected.Length; i++)
                {
                    Assert.AreEqual(expected[i], actual[i]);
                }

            }
        }
    }
}
