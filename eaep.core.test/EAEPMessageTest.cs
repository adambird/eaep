using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace eaep.test
{
    
    
    /// <summary>
    ///This is a test class for EAEPMessageTest and is intended
    ///to contain all EAEPMessageTest Unit Tests
    ///</summary>
	[TestClass]
	public class EAEPMessageTest
	{
		/// <summary>
		///A test for TimeStamp
		///</summary>
		[TestMethod]
		public void TimeStampTest()
		{
            // Arrange
			var target = new EAEPMessage(); 
			var expected = new DateTime();
		    target.TimeStamp = expected;

            // Act
			var actual = target.TimeStamp;

            // Assert
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for Item
		///</summary>
		[TestMethod]
		public void ItemTest()
		{
            // Arrange
			var target = new EAEPMessage();
			const string param = "user";
			const string expected = "user@company.com";
		    target[param] = expected;

            // Act
			string actual = target[param];

            // Assert
			Assert.AreEqual(expected, actual);
		}

        /// <summary>
        ///A test for Item with header field name
        ///</summary>
        [TestMethod]
        public void ItemHeaderTest()
        {
            // Arrange
            var target = new EAEPMessage();
            const string expected = "MyApp";
            target.Application = "MyApp";
            target.Application = expected;

            // Act
            var actual = target.Application;

            // Assert
            Assert.AreEqual(expected, actual);
        }


		/// <summary>
		///A test for Host
		///</summary>
		[TestMethod]
		public void HostTest()
		{
            // Arrange
			var target = new EAEPMessage();
			const string expected = "app1";
		    target.Host = expected;

            // Act
			var actual = target.Host;

            // Assert
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for Event
		///</summary>
		[TestMethod]
		public void EventTest()
		{
			var target = new EAEPMessage();
			const string expected = "purchase";
		    target.Event = expected;
			string actual = target.Event;
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for Application
		///</summary>
		[TestMethod]
		public void ApplicationTest()
		{
            // Arrange
			var target = new EAEPMessage(); 
			const string expected = "purchase";
		    target.Application = expected;

            // Act
			var actual = target.Application;

            // Assert
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Equals Test
		/// </summary>
		[TestMethod]
		public void EqualsTest_Equal()
		{
            // Arrange
			var message1 = new EAEPMessage
			               {
			                   TimeStamp = DateTime.Now,
			                   Host = "app1",
			                   Application = "ecommerce",
			                   Event = "purchase"
			               };
		    message1["value"] = "25.00";
			message1["currency"] = "EUR";
			message1["user"] = "user@company.com";

			var message2 = new EAEPMessage
			               {
			                   TimeStamp = message1.TimeStamp,
			                   Host = "app1",
			                   Application = "ecommerce",
			                   Event = "purchase"
			               };
		    message2["value"] = "25.00";
			message2["currency"] = "EUR";
			message2["user"] = "user@company.com";

            // Assert
			Assert.AreEqual(message1, message2);
		}

		/// <summary>
		/// Equals Test - Not
		/// </summary>
		[TestMethod]
		public void EqualsTest_NotEqual()
		{
            // Arrange
			var message1 = new EAEPMessage
			               {
			                   Host = "app1", 
                               Application = "ecommerce", 
                               Event = "purchase"
			               };
		    message1["value"] = "25.00";
			message1["currency"] = "EUR";
			message1["user"] = "user@company.com";

			var message2 = new EAEPMessage
			               {
			                   Host = "app1", 
                               Application = "ecommerce", 
                               Event = "purchase"
			               };
		    message2["value"] = "25.00";
			message2["currency"] = "EUR";
			message2["product"] = "SMS 200";

            // Assert
			Assert.AreNotEqual(message1, message2);
		}

		/// <summary>
		/// Load 
		///</summary>
		[TestMethod]
		public void LoadTest()
		{
            // Arrange
			var expectedMessage = new EAEPMessage
			                      {
			                          Version = "1.0",
			                          Host = "app1",
			                          Application = "ecommerce",
			                          Event = "purchase",
			                          TimeStamp =
			                              DateTime.ParseExact("12-03-2007-12:33:59.123", EAEPMessage.TIMESTAMP_FORMAT,
			                                                  CultureInfo.InvariantCulture)
			                      };
		    expectedMessage["value"] = "25.00";
			expectedMessage["currency"] = "EUR";
			expectedMessage["user"] = "user@company.com";

			const string serialisedMessage = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nvalue=25.00\r\ncurrency=EUR\r\nuser=user@company.com\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

			var message = new EAEPMessage();

            // Act
			message.Load(serialisedMessage);

            // Assert
			Assert.AreEqual(expectedMessage, message);
		}

		///<summary>
		/// Confirm serialise message = deserialised message
		/// </summary>
		[TestMethod]
		public void LoadSerialisedTest()
		{
            // Arrange
			var expectedMessage = new EAEPMessage("host1", "app1", "loadpage")
			                      {
			                          TimeStamp = DateTime.Now
			                      };
		    expectedMessage["user"] = "user@company.com";

            // Act
			var actualMessage = new EAEPMessage(expectedMessage.ToString());

            // Assert
			Assert.AreEqual(expectedMessage, actualMessage);
		}

		///<summary>
		/// ToString 
		/// </summary>
		[TestMethod]
		public void ToStringTest()
		{
            // Arrange
			var message = new EAEPMessage
			              {
			                  Version = "1.0",
			                  Host = "app1",
			                  Application = "ecommerce",
			                  Event = "purchase",
			                  TimeStamp =
			                      DateTime.ParseExact("12-03-2007-12:33:59.123", EAEPMessage.TIMESTAMP_FORMAT,
			                                          CultureInfo.InvariantCulture)
			              };
		    message["value"] = "25.00";
			message["currency"] = "EUR";
			message["user"] = "user@company.com";

			const string expectedString = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nvalue=25.00\r\ncurrency=EUR\r\nuser=user%40company.com\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

            // Act / Assert
			Assert.AreEqual(expectedString, message.ToString());
		}

		/// <summary>
		///A test for EAEPMessage Constructor
		///</summary>
		[TestMethod]
		public void EsAEPMessageConstructorTest()
		{
            // Arrange
			var expectedMessage = new EAEPMessage
			                      {
			                          Version = "1.0",
			                          Host = "app1",
			                          Application = "ecommerce",
			                          Event = "purchase",
			                          TimeStamp =
			                              DateTime.ParseExact("12-03-2007-12:33:59.123", EAEPMessage.TIMESTAMP_FORMAT,
			                                                  CultureInfo.InvariantCulture)
			                      };
		    expectedMessage["value"] = "25.00";
			expectedMessage["currency"] = "EUR";
			expectedMessage["user"] = "user@company.com";

			const string serialisedMessage = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nvalue=25.00\r\ncurrency=EUR\r\nuser=user@company.com\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

            // Act
			var message = new EAEPMessage(serialisedMessage);

            // Assert
			Assert.AreEqual(expectedMessage, message);
		}

		///<summary>
		///Check for presence of a parameter
		///</summary>
		[TestMethod]
		public void ContainsParameterTest()
		{
            // Arrange
			var message = new EAEPMessage
			              {
			                  Host = "app1", 
                              Application = "ecommerce", 
                              Event = "purchase"
			              };
		    message["value"] = "25.00";
			message["currency"] = "EUR";
			message["user"] = "user@company.com";

            // Act / Assert
			Assert.IsTrue(message.ContainsParameter("currency"));
			Assert.IsFalse(message.ContainsParameter("bilge"));

		}

		///<summary>
		/// Check parameter names do not contain reserved field names
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParameterNameValidationTest_Message()
		{
            // Arrange / Act
			var message = new EAEPMessage();
			message[EAEPMessage.FIELD_MESSAGE] = "bilge";			
		}

		///<summary>
		/// Check parameter names do not contain reserved field names
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParameterNameValidationTest_Application()
		{
            // Arrange / Act
			var message = new EAEPMessage();
			message[EAEPMessage.FIELD_APPLICATION] = "bilge";
		}

		///<summary>
		/// Check parameter names do not contain reserved field names
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParameterNameValidationTest_Host()
		{
            // Arrange / Act
			var message = new EAEPMessage();
			message[EAEPMessage.FIELD_HOST] = "bilge";
		}

		///<summary>
		/// Check parameter names do not contain reserved field names
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParameterNameValidationTest_Timestamp()
		{
            // Arrange / Act
			var message = new EAEPMessage();
			message[EAEPMessage.FIELD_TIMESTAMP] = "bilge";
		}

		///<summary>
		/// Check parameter names do not contain reserved field names
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ParameterNameValidationTest_Event()
		{
            // Arrange / Act
			var message = new EAEPMessage();
			message[EAEPMessage.FIELD_EVENT] = "bilge";
		}

        [TestMethod]
        public void AddAMessageIDParameter()
        {
            // Arrange
            var messageID = Guid.NewGuid();

            var message = new EAEPMessage("myhost", "myapp", "myevent");

            // Act
            message["messageID"] = messageID.ToString();

            // Assert
            Assert.AreEqual(messageID.ToString(), message["messageID"]);
        }

    
        [TestMethod]
        public void CaseInsensitiveParameters()
        {
            //  arrange
            var message = new EAEPMessage();
            message["monkey"] = "helloWorld";

            //  act
            var containsParameter = message.ContainsParameter("MONKEY");

            //  assert
            Assert.IsTrue(containsParameter);
        }

        [TestMethod]
        public void CaseInsensitiveParameters_SetDifferently()
        {
            //  arrange
            var message = new EAEPMessage();

            const string expectedValue = "helloWorld";
            message["monkey"] = expectedValue;

            //  act
            var actualValue = message["MONKEY"];

            //  assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void WithParamValueContainingNewLine_SerializesThenDeserializesWithoutError_AndWithOriginalValue()
        {
            // arrange
            var paramValueWithNewLine = @"this value
is on more than one line";
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");

            // act
            message["param"] = paramValueWithNewLine;

            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.AreEqual(paramValueWithNewLine, deserializedMessage["param"]);
        }

        [TestMethod]
        public void WithParamValueWithParamDelimiter_SerializesThenDeserializesWithoutError_AndWithOriginalValue()
        {
            // arrange
            var paramValueWithDelimiter = @"start of value=end of value after delimiter";
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");

            // act
            message["param"] = paramValueWithDelimiter;

            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.AreEqual(paramValueWithDelimiter, deserializedMessage["param"]);
        }

        [TestMethod]
        public void WithParamValueWithEndOfMessageCharacters_SerializesThenDeserializesWithoutError_AndWithOriginalValue()
        {
            // arrange
            var paramValueWithEndOfMessageCharacters = @"start of value---end of value after end of message";
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");

            // actl
            message["param"] = paramValueWithEndOfMessageCharacters;

            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.AreEqual(paramValueWithEndOfMessageCharacters, deserializedMessage["param"]);
        }

        [TestMethod]
        public void WithNonUrlEncoded_WithoutCarrageReturnSerializesEaep_ThenDeserializesWithoutError_AndWithOriginalValue()
        {
            const string paramValue = "My value has spaces";
            const string serialisedMessage = "EAEP 1.0 12-03-2007-12:33:59.123\r\napp1 ecommerce purchase\r\nparam=" + 
                paramValue  + "\r\n" + EAEPMessage.END_OF_MESSAGE + "\r\n";

            var deserializedMessage = new EAEPMessage(serialisedMessage);

            // assert
            Assert.AreEqual(paramValue, deserializedMessage["param"]);
        }

        [TestMethod]
        public void WithComplexParameterValue_EaepMessageSerializes_AndDeserializesOk()
        {
            // arrange
            var paramValue = "This=Parameter Is\r\nComplex";
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");

            // act
            message["param"] = paramValue;

            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.AreEqual(paramValue, deserializedMessage["param"]);
        }

        [TestMethod]
        public void ExampleMessage_EaepMessageSerializesOk()
        {
            var paramValue = @"Jim Parker, 447974326623, 1 pax, GVA-Landry, 
newcastle - 6465 - 13:35, paid";
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");

            // act
            message["param"] = paramValue;

            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.AreEqual(paramValue, deserializedMessage["param"]);

        }

        [TestMethod] 
        public void SerializeAndDeserialize_EaepMessage_WithNullParameterValue()
        {
            // arrange
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");
            message["param"] = null;

            // act
            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.IsTrue(string.IsNullOrEmpty(deserializedMessage["param"]));
        }

        [TestMethod]
        public void SerializeAndDeserialize_EaepMessage_WithEmptyStringParameterValue()
        {
            // arrange
            var message = new EAEPMessage(DateTime.UtcNow, "host", "app", "event");
            message["param"] = string.Empty;

            // act
            var serialized = message.ToString();

            var deserializedMessage = new EAEPMessage(serialized);

            // assert
            Assert.IsTrue(string.IsNullOrEmpty(deserializedMessage["param"]));
        }


	}
}
