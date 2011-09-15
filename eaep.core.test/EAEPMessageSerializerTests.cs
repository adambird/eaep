using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eaep.test
{
    [TestClass]
    public class EAEPMessageSerializerTests
    {
        [TestMethod]
        public void Deserialize_WithValidSerializedEaepMessageString_ReturnsEaepMessage()
        {
            // arrange
            const string serialisedMessage = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nvalue=25.00\r\ncurrency=EUR\r\nuser=user@company.com\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

            var serializer = new EAEPMessageSerializer();

            // act
            var message = serializer.Deserialize(Encoding.UTF8.GetBytes(serialisedMessage));

            // assert
            Assert.IsNotNull(message);
            Assert.AreEqual("ecommerce", message.Application);
        }

        [TestMethod]
        public void Deserialize_WithInValidSerializedEaepMessageString_ReturnsNull()
        {
            // arrange
            const string serialisedMessageIncompleteParam = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nvalue=25.00\r\ncurrency=EUR\r\nuser\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

            var serializer = new EAEPMessageSerializer();

            // act
            var message = serializer.Deserialize(Encoding.UTF8.GetBytes(serialisedMessageIncompleteParam));

            // assert
            Assert.IsNull(message);
        }
    }
}
