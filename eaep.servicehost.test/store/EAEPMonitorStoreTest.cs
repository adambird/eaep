using System;
using eaep.servicehost.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eaep.servicehost.test.store
{

    /// <summary>
    ///This is a test class for EAEPMonitorStoreTest and is intended
    ///to contain all EAEPMonitorStoreTest Unit Tests
    ///</summary>
    [TestClass]
    [Ignore]
    public class EAEPMonitorStoreTest
    {
        /// <summary>
        ///A test for GetMessage
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_OneMessage_SearchHeaders()
        {
            const string appName = "ecommerce";

            var message = new EAEPMessage("app1", appName, "purchase");

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                var messages = monitorStore.GetMessages(string.Format("{0}:{1}",EAEPMessage.FIELD_APPLICATION, "e*"));

                Assert.AreEqual(0, messages.Count);

                monitorStore.PushMessage(message);

                messages = monitorStore.GetMessages(string.Format("{0}:{1}",EAEPMessage.FIELD_APPLICATION, appName));

                Assert.AreEqual(1, messages.Count);
                Assert.AreEqual(message, messages[0]);
            }
        }

        /// <summary>
        ///Test result from GetMessages
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_MultipleMessage_SearchHeaders()
        {
            const string app1 = "app1";
            const string app2 = "app2";

            var message1 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 0), "host1", app1, "purchase");
            var message2 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 10), "host1", app2, "purchase");
            var message3 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 20), "host1", app1, "purchase");

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);

                var messages = monitorStore.GetMessages(string.Format("{0}:{1}",EAEPMessage.FIELD_APPLICATION, app1));

                // remember results are in reversed order
                Assert.AreEqual(2, messages.Count);
                Assert.AreEqual(message3, messages[0]);
                Assert.AreEqual(message1, messages[1]);

                messages = monitorStore.GetMessages(string.Format("{0}:{1}",EAEPMessage.FIELD_APPLICATION, app2));

                Assert.AreEqual(1, messages.Count);
                Assert.AreEqual(message2, messages[0]);
            }
        }

        /// <summary>
        ///A test for PushMessage
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_MultipleMessage_SearchHeaders_SearchString()
        {
            const string app1 = "app1";
            const string app2 = "app2";

            var message1 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 0), "host1", app1, "purchase");
            var message2 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 10), "host1", app2, "login");
            var message3 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 20), "host1", app1, "purchase");
            var message4 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 30), "host1", app2, "purchase");

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);
                monitorStore.PushMessage(message4);

                var messages = monitorStore.GetMessages(string.Format("{0}:{1}", EAEPMessage.FIELD_EVENT, "purchase"));

                Assert.AreEqual(3, messages.Count);
                Assert.AreEqual(message4, messages[0]);
                Assert.AreEqual(message3, messages[1]);
                Assert.AreEqual(message1, messages[2]);
            }
        }

        /// <summary>
        ///A test for GetMessages
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_MultipleMessage_SearchParameters_SearchString()
        {
            const string app1 = "app1";
            const string app2 = "app2";
            const string userParam = "user";
            const string user1 = "user1@company.com";
            const string user2 = "user2@company.com";

            var message1 = new EAEPMessage("host1", app1, "purchase");
            message1[userParam] = user2;
            var message2 = new EAEPMessage("host1", app2, "login");
            message2[userParam] = user1;
            var message3 = new EAEPMessage("host1", app1, "purchase");
            message3[userParam] = user1;
            var message4 = new EAEPMessage("host1", app2, "purchase");
            message4[userParam] = user2;

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);
                monitorStore.PushMessage(message4);

                var messages = monitorStore.GetMessages(string.Format("{0}:{1}", userParam, user1));

                Assert.AreEqual(2, messages.Count);
                Assert.AreEqual(message2, messages[0]);
                Assert.AreEqual(message3, messages[1]);
            }
        }

        /// <summary>
        ///A test for GetMessages
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_Since()
        {
            // Arrange
            const string app1 = "app1";
            const string app2 = "app2";

            var message1 = new EAEPMessage("host1", app1, "purchase")
                           {
                               TimeStamp = DateTime.Now.AddMinutes(-10)
                           };
            var message2 = new EAEPMessage("host1", app2, "login")
                           {
                               TimeStamp = DateTime.Now.AddMinutes(-5)
                           };
            var message3 = new EAEPMessage("host1", app1, "purchase")
                           {
                               TimeStamp = DateTime.Now.AddMinutes(-2)
                           };
            var message4 = new EAEPMessage("host1", app1, "purchase")
                           {
                               TimeStamp = DateTime.Now
                           };

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);
                monitorStore.PushMessage(message4);

                // Act
                var messages = monitorStore.GetMessages(
                    DateTime.Now.AddMinutes(-8),
                    string.Format("{0}:{1}", EAEPMessage.FIELD_APPLICATION, app1)
                    );

                // Assert
                Assert.AreEqual(2, messages.Count);
                Assert.AreEqual(message4, messages[0]);
                Assert.AreEqual(message3, messages[1]);
            }
        }

        [TestMethod]
        public void GetMessagesTest_Between()
        {
            // Arrange
            const string app1 = "app1";
            const string app2 = "app2";

            var message1 = new EAEPMessage(new DateTime(2009, 9, 12, 12, 0, 0), "host1", app1, "purchase");
            var message2 = new EAEPMessage(new DateTime(2009, 9, 12, 12, 10, 0), "host1", app2, "login");
            var message3 = new EAEPMessage(new DateTime(2009, 9, 12, 12, 20, 0), "host1", app1, "purchase");
            var message4 = new EAEPMessage(new DateTime(2009, 9, 12, 12, 40, 0), "host1", app1, "purchase");

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);
                monitorStore.PushMessage(message4);

                // Act
                var messages = monitorStore.GetMessages(
                    new DateTime(2009, 9, 12, 12, 5, 0),
                    new DateTime(2009, 9, 12, 12, 22, 0),
                    string.Format("{0}:{1}", EAEPMessage.FIELD_HOST, "host1")
                    );

                // Assert
                Assert.AreEqual(2, messages.Count);
                Assert.AreEqual(message3, messages[0]);
                Assert.AreEqual(message2, messages[1]);
            }
        }

        /// <summary>
        ///Testing the search based on the event field, found as a bug
        ///</summary>
        [TestMethod]
        public void GetMessagesTest_CaseSensitivity()
        {
            var message1 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 0), "host1", "app1", "purchase");
            var message2 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 10), "host1", "app2", "Purchase");
            var message3 = new EAEPMessage(new DateTime(2009, 9, 17, 12, 0, 20), "host1", "app1", "Purchase");

            using (var monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString))
            {
                monitorStore.PushMessage(message1);
                monitorStore.PushMessage(message2);
                monitorStore.PushMessage(message3);

                var messages = monitorStore.GetMessages(string.Format("{0}:{1}", EAEPMessage.FIELD_EVENT, "Purchase"));

                Assert.AreEqual(2, messages.Count);
                Assert.AreEqual(message3, messages[0]);
                Assert.AreEqual(message2, messages[1]);

                messages = monitorStore.GetMessages(string.Format("{0}:{1}",EAEPMessage.FIELD_EVENT, "purchase"));

                Assert.AreEqual(1, messages.Count);
                Assert.AreEqual(message1, messages[0]);
            }
        }
    }
}
