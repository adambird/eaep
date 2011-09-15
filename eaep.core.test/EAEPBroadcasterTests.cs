using System;
using System.Text;
using eaep.multicast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Sockets;

namespace eaep.test
{
    /// <summary>
    /// Summary description for EAEPBroadcasterTests
    /// </summary>
    [TestClass]
    public class EAEPBroadcasterTests
    {        
        [TestMethod]
        public void Constructor_WithNullApplication_ThrowsArgumentNullException()
        {
            try
            {
                new EAEPBroadcaster("host", null, new MulticastSettings("192.168.0.1", 21, 1));
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void Constructor_WithNullHost_ThrowsArgumentNullException()
        {
            try
            {
                new EAEPBroadcaster(null, "application", new MulticastSettings("192.168.0.1", 21, 1));
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Constructor_WithEmptyHost_ThrowsArgumentException()
        {
            try
            {
                new EAEPBroadcaster("", "application", new MulticastSettings("192.168.0.1", 21, 1));
                Assert.Fail();
            }
            catch(ArgumentException)
            {
            }
        }


        [TestMethod]
        public void Constructor_WithEmptyApplication_ThrowsArgumentException()
        {
            try
            {
                new EAEPBroadcaster("host", "", new MulticastSettings("192.168.0.1", 21, 1));
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                
            }
        }

        [TestMethod]
        public void Constructor_WithNullMulticastSettings_ThrowsArgumentException()
        {
            try
            {
                MulticastSettings settings = null;
                new EAEPBroadcaster("host", "application", settings);
                Assert.Fail();
            }
            catch(ArgumentException)
            {

            }
        }

        [TestMethod]
        public void LogEvent()
        {
            //  arrange
            const string application = "signUp";
            const string host = "local";
            const string eventName = "buttonClick";

            var timeStamp = new DateTime(2000, 1, 1);

            var expectedMessage = new EAEPMessage(timeStamp,
                host,
                application,
                eventName);

            expectedMessage.Parameters["name1"] = "value1";
            expectedMessage.Parameters["name2"] = "value2";

            var expectedBytes = Encoding.UTF8.GetBytes(expectedMessage.ToString());
            var mockMulticaster = new Mock<IMulticastSender>();
            var broadcaster = new EAEPBroadcaster(host, application, mockMulticaster.Object);

            //  act
            broadcaster.LogEvent(timeStamp,
                eventName,
                new EventParameter("name1", "value1"),
                new EventParameter("name2", "value2"));

            //  assert
            mockMulticaster.Verify(m => m.Send(expectedBytes), Times.Once());
        }

        [TestMethod]
        public void LogEvent_WithNullEventName_ThrowsArgumentNullException()
        {
            //  arrange
            var broadcaster = new EAEPBroadcaster("host", "application", new MulticastSettings("192.168.0.1", 21, 1));
            
            try
            {
                //  act
                broadcaster.LogEvent(null);  
                
                //  assert
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
                
            }
        }

        [TestMethod]
        public void LogEvent_WithEmptyEventName_ThrowsArgumentException()
        {
            //  arrange
            var broadcaster = new EAEPBroadcaster("host", "application", new MulticastSettings("192.168.0.1", 21, 1));

            try
            {
                //  act
                broadcaster.LogEvent("");

                //  assert
                Assert.Fail();
            }
            catch(ArgumentException)
            {

            }
        }

        [TestMethod]
        public void LogEvent_IMulticastThrowsSocketException()
        {
            //  arrange
            var mockMulticaster = new Mock<IMulticastSender>();
            mockMulticaster.Setup(m => m.Send(It.IsAny<byte[]>())).Throws(new SocketException());

            var broadcaster = new EAEPBroadcaster("host", "application", mockMulticaster.Object);

            try
            {
                //  act
                broadcaster.LogEvent("someEvent");
            }
            catch(SocketException)
            {
                //  assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LogEvent_IMulticastThrowsObjectDisposedException()
        {
            //  arrange
            var mockMulticaster = new Mock<IMulticastSender>();
            mockMulticaster.Setup(m => m.Send(It.IsAny<byte[]>())).Throws(new ObjectDisposedException("testobject"));

            var broadcaster = new EAEPBroadcaster("host", "application", mockMulticaster.Object);

            try
            {
                broadcaster.LogEvent("someEvent");
            } 
            catch(ObjectDisposedException)
            {
                Assert.Fail();
            }
        }
    }
}
