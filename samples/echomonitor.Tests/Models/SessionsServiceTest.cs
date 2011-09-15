using echomonitor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System;
using eaep;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using Moq;
using eaep.store;

namespace echomonitor.Tests.Models
{
    
    
    /// <summary>
    ///This is a test class for SessionsServiceTest and is intended
    ///to contain all SessionsServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SessionsServiceTest
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
        ///A test for GetDaySummary
        ///</summary>
        [TestMethod()]
        public void GetDaySummaryTest()
        {
            // Arrange
            int[] expectedResult = new int[96];

            CountResult[] countResult = new CountResult[] 
            { 
                new CountResult() {TimeSlice = 2, FieldValue = "user1", Count = 2},
                new CountResult() {TimeSlice = 2, FieldValue = "user2", Count = 4},
                new CountResult() {TimeSlice = 3, FieldValue = "user1", Count = 2},
                new CountResult() {TimeSlice = 3, FieldValue = "user3", Count = 2},
                new CountResult() {TimeSlice = 3, FieldValue = "user2", Count = 2},
                new CountResult() {TimeSlice = 4, FieldValue = "user2", Count = 2},
                new CountResult() {TimeSlice = 5, FieldValue = "user3", Count = 2},
                new CountResult() {TimeSlice = 5, FieldValue = "user1", Count = 2},
            };

            expectedResult[2] = 2;
            expectedResult[3] = 3;
            expectedResult[4] = 1;
            expectedResult[5] = 2;

            SessionsSummary expected = new SessionsSummary();
            expected.data = expectedResult;

            string applicationName = "awebapp";
            DateTime day = DateTime.Now;

            var eaepClient = new Mock<IEAEPMonitorClient>();
            eaepClient
                .Setup(x => x.Count("app:awebapp", day.Date, day.Date.AddDays(1), expectedResult.Length, EAEPMessage.PARAM_USER))
                .Returns(countResult);

            SessionsService target = new SessionsService(eaepClient.Object);

            // Act
            SessionsSummary actual = target.GetDaySummary(applicationName, day, expectedResult.Length);

            // Assert
            object[] actualResult = (object[])actual.data;
            Assert.AreEqual(expectedResult.Length, actualResult.Length);
            for (int i = 0; i < expectedResult.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], ((int[])actualResult[i])[1]);
            }
        }

        [TestMethod]
        public void ActiveUsersTest()
        {
            // Arrange
            string[] expected = new string[] { "user1", "user2", "user3" };
            string query = "app:echo";
            string field = EAEPMessage.PARAM_USER;

            var eaepClient = new Mock<IEAEPMonitorClient>();
            
            eaepClient
                .Setup(x => x.Distinct(query, It.IsAny<DateTime>(), It.IsAny<DateTime>(), field))
                .Returns(expected);

            SessionsService target = new SessionsService(eaepClient.Object);

            // Act
            string[] actual = target.ActiveUsers("echo");

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

    }
}
