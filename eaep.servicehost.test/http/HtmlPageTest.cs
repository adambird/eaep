using System.IO;
using System.Text;
using eaep.servicehost.http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eaep.servicehost.test.http
{
    
    
    /// <summary>
    ///This is a test class for HtmlPageTest and is intended
    ///to contain all HtmlPageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HtmlPageTest
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
        ///A test for Handle
        ///</summary>
        [TestMethod()]
        public void HandleTest()
        {
            string pageName = "search";
            byte[] content = Encoding.UTF8.GetBytes("<html><body>Search me</body></html>");

            MockResourceRepository resourceRepository = new MockResourceRepository();
            resourceRepository.LoadResource(pageName, content);

            HtmlPage target = new HtmlPage();

            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.ResourceName).Returns(pageName);
            request.SetupGet(x => x.Extension).Returns("");
            request.SetupGet(x => x.Query).Returns("");

            var response = new Mock<IServiceResponse>();
            response.SetupProperty(x => x.ContentType);
            response.SetupProperty(x => x.StatusCode);

            MemoryStream contentStream = new MemoryStream();
            response.SetupGet(x => x.ContentStream).Returns(contentStream);

            target.Handle(request.Object, response.Object, resourceRepository);

            Assert.AreEqual("text/html", response.Object.ContentType);
            Assert.AreEqual(200, response.Object.StatusCode);

            byte[] actualContent = contentStream.ToArray();
            Assert.AreEqual(content.Length, actualContent.Length);
            for (int i = 0; i < content.Length; i++)
            {
                Assert.AreEqual(content[i], actualContent[i]);
            }
        }
    }
}
