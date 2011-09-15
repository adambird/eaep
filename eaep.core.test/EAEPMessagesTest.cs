using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eaep.test
{
    /// <summary>
    ///This is a test class for EAEPMessagesTest and is intended
    ///to contain all EAEPMessagesTest Unit Tests
    ///</summary>
	[TestClass]
	public class EAEPMessagesTest
	{
		/// <summary>
		///A test for ToString
		///</summary>
		[TestMethod]
		public void ToStringTest()
		{
            // Arrange
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
			
			var target = new EAEPMessages {message1, message2, message3, message4};

		    var expected = string.Format("{0}{1}{2}{3}", message1, message2, message3, message4);

            // Act
			var actual = target.ToString();

            // Assert
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for Load
		///</summary>
		[TestMethod]
		public void LoadTest()
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

			var expected = new EAEPMessages {message1, message2, message3, message4};

		    var actual = new EAEPMessages();
			actual.Load(expected.ToString());

			Assert.AreEqual(expected.Count, actual.Count);
			for (var i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod]
        public void LoadConstructorTest()
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

            var expected = new EAEPMessages {message1, message2, message3, message4};

            var actual = new EAEPMessages(expected.ToString());

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

	}
}
