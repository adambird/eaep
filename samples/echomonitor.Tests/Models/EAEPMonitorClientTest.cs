using echomonitor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System;

namespace echomonitor.Tests.Models
{
    
    
    /// <summary>
    ///This is a test class for EAEPMonitorClientTest and is intended
    ///to contain all EAEPMonitorClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EAEPMonitorClientTest
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
        ///A test for ConstructCountURITest
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void ConstructCountURITest()
        {
            // Arrange 
            Configuration.MonitorURI = "http://my-host:8085/";

            string query = "app:app1";
            DateTime rangeFrom = new DateTime(2009, 9, 12, 0, 1, 0);
            DateTime rangeTo = new DateTime(2009, 9, 12, 1, 1, 0);
            int timeSlices = 24;
            string groupBy = "user";

            string expected = string.Format("{0}count.json?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                Configuration.MonitorURI,
                eaep.http.Constants.QUERY_STRING_QUERY, query,
                eaep.http.Constants.QUERY_STRING_FROM, rangeFrom.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_TO, rangeTo.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_TIMESLICES, timeSlices.ToString("0"),
                eaep.http.Constants.QUERY_STRING_GROUPBY, groupBy);

            // Act
            string actual = EAEPMonitorClient.ConstructCountURI(query, rangeFrom, rangeTo, timeSlices, groupBy);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ConstructCountURITest with Null GroupBy
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void ConstructCountURITest_NullGroupBy()
        {
            // Arrange 
            Configuration.MonitorURI = "http://my-host:8085/";

            string query = "app:app1";
            DateTime rangeFrom = new DateTime(2009, 9, 12, 0, 1, 0);
            DateTime rangeTo = new DateTime(2009, 9, 12, 1, 1, 0);
            int timeSlices = 24;

            string expected = string.Format("{0}count.json?{1}={2}&{3}={4}&{5}={6}&{7}={8}",
                Configuration.MonitorURI,
                eaep.http.Constants.QUERY_STRING_QUERY, query,
                eaep.http.Constants.QUERY_STRING_FROM, rangeFrom.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_TO, rangeTo.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_TIMESLICES, timeSlices.ToString("0"));

            // Act
            string actual = EAEPMonitorClient.ConstructCountURI(query, rangeFrom, rangeTo, timeSlices, null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConstructDistinctURITest()
        {
            // Arrange 
            Configuration.MonitorURI = "http://my-host:8085/";

            string query = "app:app1";
            DateTime rangeFrom = new DateTime(2009, 9, 12, 0, 1, 0);
            DateTime rangeTo = new DateTime(2009, 9, 12, 1, 1, 0);
            string field = "blge";

            string expected = string.Format("{0}distinct.json?{1}={2}&{3}={4}&{5}={6}&{7}={8}",
                Configuration.MonitorURI,
                eaep.http.Constants.QUERY_STRING_QUERY, query,
                eaep.http.Constants.QUERY_STRING_FROM, rangeFrom.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_TO, rangeTo.ToString(eaep.http.Constants.FORMAT_DATETIME),
                eaep.http.Constants.QUERY_STRING_FIELD, field);

            // Act
            string actual = EAEPMonitorClient.ConstructDistinctURI(query, rangeFrom, rangeTo, field);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
