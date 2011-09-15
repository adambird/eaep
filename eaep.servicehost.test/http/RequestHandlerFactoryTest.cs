using eaep.servicehost.http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eaep.servicehost.test.http
{
    
    
    /// <summary>
    ///This is a test class for RequestHandlerFactoryTest and is intended
    ///to contain all RequestHandlerFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RequestHandlerFactoryTest
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
        ///A test for GetHandler
        ///</summary>
        [TestMethod()]
        public void GetHandlerTest_Default()
        {
            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.Extension).Returns("");
            request.SetupGet(x => x.ResourceName).Returns(string.Empty);
            request.SetupGet(x => x.Query).Returns("");

            IRequestHandler actual;
            actual = RequestHandlerFactory.GetHandler(request.Object, null);
            Assert.IsInstanceOfType(actual, typeof(SearchPage));
        }

        [TestMethod()]
        public void GetHandlerTest_Service()
        {
            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.ResourceName).Returns("bilge.json");
            request.SetupGet(x => x.Extension).Returns(".json");
            request.SetupGet(x => x.Query).Returns("");

            IRequestHandler actual;
            actual = RequestHandlerFactory.GetHandler(request.Object, null);
            Assert.IsInstanceOfType(actual, typeof(SearchService));
        }


        [TestMethod()]
        public void GetHandlerTest_CountService()
        {
            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.ResourceName).Returns("count.json");
            request.SetupGet(x => x.Extension).Returns(".json");
            request.SetupGet(x => x.Query).Returns("");

            IRequestHandler actual;
            actual = RequestHandlerFactory.GetHandler(request.Object, null);
            Assert.IsInstanceOfType(actual, typeof(CountService));
        }


        [TestMethod()]
        public void GetHandlerTest_EventService()
        {
            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.ResourceName).Returns("event");
            request.SetupGet(x => x.Extension).Returns(string.Empty);
            request.SetupGet(x => x.Query).Returns("");

            IRequestHandler actual;
            actual = RequestHandlerFactory.GetHandler(request.Object, null);
            Assert.IsInstanceOfType(actual, typeof(EventService));
        }

        [TestMethod()]
        public void GetHandlerTest_DistinctService()
        {
            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.ResourceName).Returns("distinct.json");
            request.SetupGet(x => x.Extension).Returns(".json");
            request.SetupGet(x => x.Query).Returns("app:echo");

            IRequestHandler actual;
            actual = RequestHandlerFactory.GetHandler(request.Object, null);
            Assert.IsInstanceOfType(actual, typeof(DistinctService));
        }
    }
}
