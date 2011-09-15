using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eaep.multicast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eaep.test
{
    [TestClass]
    public class EAEPNodeTests
    {
        public interface IEaepMessageReceiver
        {
            void OnMessageReceived(EAEPMessage message);
        }

        private readonly MulticastSettings _multicastSettings = new MulticastSettings("127.0.0.1", 60000, 0);
        private Mock<IEAEPMessageSerializer> _mockEaepMessageSerializer;
        private Mock<IMulticast> _mockMulticaster;
        private Mock<IEaepMessageReceiver> _mockMessageReceiver;


        private EAEPNode _node;

        [TestInitialize]
        public void Setup()
        {
            _mockEaepMessageSerializer = new Mock<IEAEPMessageSerializer>();
            _mockMulticaster = new Mock<IMulticast>();
            _mockMessageReceiver = new Mock<IEaepMessageReceiver>();

            _node = new EAEPNode(_multicastSettings, _mockMulticaster.Object, _mockEaepMessageSerializer.Object);
        }

        [TestMethod]
        public void WhenNodeReceivesInvalidEaepNodeString_DoesNotThrowException()
        {
            // arrange
            var data = new byte[0];

            _mockEaepMessageSerializer
                .Setup(ems => ems.Deserialize(data))
                .Returns((EAEPMessage) null);

            _node.MessageReceived += _mockMessageReceiver.Object.OnMessageReceived;
            
            // act           
            _mockMulticaster.Raise(mc => mc.DataReceived += null, data);
            
            // assert 
            _mockEaepMessageSerializer
                .Verify(ems => ems.Deserialize(data));
        }

        [TestMethod]
        public void WhenNodeReceivesValidEaepNodeString_DeserializesEaepMessage_CallsHandlerWithMessage()
        {
            // arrange
            var data = new byte[0];
            var message = new EAEPMessage();

            _mockEaepMessageSerializer
                .Setup(ems => ems.Deserialize(data))
                .Returns(message);

            _node.MessageReceived += _mockMessageReceiver.Object.OnMessageReceived;

            // act           
            _mockMulticaster.Raise(mc => mc.DataReceived += null, data);

            // assert 
            _mockMessageReceiver
                .Verify(mr => mr.OnMessageReceived(message));
        }

        [TestMethod]
        public void WhenNodeReceivesData_ButThereIsNoCallBack_DoesNotDserializeData()
        {
            // arrange
            var data = new byte[0];

            // act           
            _mockMulticaster.Raise(mc => mc.DataReceived += null, data);

            // assert
            _mockEaepMessageSerializer
                .Verify(ems => ems.Deserialize(It.IsAny<byte[]>()), Times.Never());

        }
    }
}
