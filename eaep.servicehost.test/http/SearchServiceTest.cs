using System.IO;
using System.Text;
using eaep.servicehost.http;
using eaep.servicehost.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace eaep.servicehost.test.http
{
    /// <summary>
    ///This is a test class for SearchServiceTest and is intended
    ///to contain all SearchServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SearchServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ProcessRequest
        ///</summary>
        [TestMethod()]
        [DeploymentItem("eaep.core.dll")]
        public void ProcessRequestTest()
        {
            // Arrange

            EAEPMessages messages = new EAEPMessages();
            messages.Add(new EAEPMessage("host01", "app01", "DoneSomething"));
            messages.Add(new EAEPMessage("host01", "app02", "DoneSomething"));
            messages.Add(new EAEPMessage("host01", "app01", "DoneSomethingElse"));

            string query = "hello";
            string expectedContent = JsonConvert.SerializeObject(messages);

            var monitor = new Mock<IEAEPMonitorStore>();
            monitor
                .Setup(x => x.GetMessages(query))
                .Returns(messages)
                .Verifiable();

            var request = new Mock<IServiceRequest>();
            request
                .SetupGet(x => x.Query)
                .Returns(query);

            var response = new Mock<IServiceResponse>();
            MemoryStream contentStream = new MemoryStream();
            response
                .SetupGet(x => x.ContentStream)
                .Returns(contentStream);

            response
                .SetupProperty(x => x.ContentType);

            SearchService target = new SearchService(monitor.Object);

            // Act
            target.ProcessRequest(request.Object, response.Object, null);

            // Assert
            Assert.AreEqual("application/json", response.Object.ContentType);

            string actualContent = Encoding.UTF8.GetString(contentStream.ToArray());

            Assert.AreEqual(expectedContent, actualContent);
        }
    }
}
