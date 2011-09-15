using System;
using System.IO;
using System.Text;
using eaep.servicehost.http;
using eaep.servicehost.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace eaep.servicehost.test.http
{

    [TestClass]
    public class CountServiceTest
    {
        /// <summary>
        ///A test for ProcessRequest
        ///</summary>
        [TestMethod]
        [DeploymentItem("eaep.core.dll")]
        public void ProcessRequestTest()
        {
            var expectedResults = new[]
            {
                new CountResult {TimeSlice = 2, FieldValue = "user1", Count = 23},
                new CountResult {TimeSlice = 2, FieldValue = "user2", Count = 12},
                new CountResult {TimeSlice = 3, FieldValue = "user1", Count = 8},
                new CountResult {TimeSlice = 3, FieldValue = "user2", Count = 7},
                new CountResult {TimeSlice = 3, FieldValue = "user3", Count = 78},
                new CountResult {TimeSlice = 4, FieldValue = "user4", Count = 12},
                new CountResult {TimeSlice = 4, FieldValue = "user1", Count = 23},
            };

            var expectedContent = JsonConvert.SerializeObject(expectedResults);

            const string expectedQuery = "app:webapp";
            var expectedFrom = new DateTime(2009, 9, 12, 0, 0, 0);
            var expectedTo = new DateTime(2009, 9, 13, 0, 0, 0);
            const int expectedTimeSlices = 6;
            const string expectedGroupBy = "user";

            var store = new Mock<IEAEPMonitorStore>();
            store
                .Setup(x => x.Count(expectedFrom, expectedTo, expectedTimeSlices, expectedGroupBy, expectedQuery))
                .Returns(expectedResults);

            var request = new Mock<IServiceRequest>();
            request.SetupGet(x => x.Query).Returns(expectedQuery);
            request
                .Setup(x => x.GetParameter(Constants.QUERY_STRING_FROM))
                .Returns(expectedFrom.ToString(Constants.FORMAT_DATETIME));

            request
                .Setup(x => x.GetParameter(Constants.QUERY_STRING_TO))
                .Returns(expectedTo.ToString(Constants.FORMAT_DATETIME));

            request
                .Setup(x => x.GetParameter(Constants.QUERY_STRING_TIMESLICES))
                .Returns(expectedTimeSlices.ToString("0"));

            request
                .Setup(x => x.GetParameter(Constants.QUERY_STRING_GROUPBY))
                .Returns(expectedGroupBy);

            var response = new Mock<IServiceResponse>();
            response.SetupProperty(x => x.ContentType);

            var contentStream = new MemoryStream();
            response.SetupGet(x => x.ContentStream).Returns(contentStream);

            var target = new CountService(store.Object);

            // Act
            target.ProcessRequest(request.Object, response.Object, new MockResourceRepository());

            // Assert
            store.VerifyAll();

            Assert.AreEqual(Constants.CONTENT_TYPE_JSON, response.Object.ContentType);

            string actualContent = Encoding.UTF8.GetString(contentStream.ToArray());

            Assert.AreEqual(expectedContent, actualContent);

            
        }
    }
}
