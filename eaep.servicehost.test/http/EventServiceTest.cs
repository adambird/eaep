using eaep.servicehost.http;
using eaep.servicehost.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eaep.servicehost.test.http
{
    
    
    /// <summary>
    ///This is a test class for CountServiceTest and is intended
    ///to contain all CountServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventServiceTest
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
        ///A test for ProcessRequest with a post
        ///</summary>
        [TestMethod()]
        [DeploymentItem("eaep.core.dll")]
        public void ProcessRequestTest_POST()
        {
            // Arrange
            EAEPMessage message = new EAEPMessage("host1", "app1", "event1");

            var store = new Mock<IEAEPMonitorStore>();
            store
                .Setup(x => x.PushMessages(It.Is<EAEPMessages>
                        (ms => ms[0].Host == message.Host && ms[0].Application == message.Application && ms[0].Event == message.Event)
                    ))
                .Verifiable();

            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.Method).Returns("POST");
            request.SetupGet(x => x.ResourceName).Returns("event");
            request.SetupGet(x => x.Body).Returns(message.ToString());

            var response = new Mock<IServiceResponse>();
            response.SetupProperty(x => x.StatusCode);

            EventService target = new EventService(store.Object);
            
            // Act
            target.ProcessRequest(request.Object, response.Object, null);

            // Assert
            store.VerifyAll();
            Assert.AreEqual(Constants.HTTP_200_OK, response.Object.StatusCode);
        }

    }
}
