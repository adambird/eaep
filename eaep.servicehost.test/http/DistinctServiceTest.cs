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
    public class DistinctServiceTest
    {
        [TestMethod]
        public void ProcessRequestTest()
        {
            // Arrange
            string field = "user";
            DateTime from = new DateTime(2009, 12, 22, 10, 0, 0);
            DateTime to = from.AddHours(1);
            string query = "app:echo";

            string[] expectedResult = new string[] { "user1", "user3", "user4" };
            string expectedContent = JsonConvert.SerializeObject(expectedResult);

            var store = new Mock<IEAEPMonitorStore>();

            store
                .Setup(x => x.Distinct(field, from, to, query))
                .Returns(expectedResult)
                .Verifiable();

            var request = new Mock<IServiceRequest>();

            request.SetupGet(x => x.Method).Returns("GET");
            request.SetupGet(x => x.Query).Returns(query);
            request.SetupGet(x => x.ResourceName).Returns("distinct.json");
            request.SetupGet(x => x.Extension).Returns(".json");

            request.Setup(x => x.GetParameter(Constants.QUERY_STRING_FIELD)).Returns(field);
            request.Setup(x => x.GetParameter(Constants.QUERY_STRING_FROM)).Returns(from.ToString(Constants.FORMAT_DATETIME));
            request.Setup(x => x.GetParameter(Constants.QUERY_STRING_TO)).Returns(to.ToString(Constants.FORMAT_DATETIME));

            var response = new Mock<IServiceResponse>();
            response.SetupProperty(x => x.ContentType);
            MemoryStream contentStream = new MemoryStream();
            response.SetupGet(x => x.ContentStream).Returns(contentStream);

            DistinctService target = new DistinctService(store.Object);

            // Act
            target.ProcessRequest(request.Object, response.Object, null);

            // Assert
            store.VerifyAll();

            Assert.AreEqual(Constants.CONTENT_TYPE_JSON, response.Object.ContentType);

            string actualContent = Encoding.UTF8.GetString(contentStream.ToArray());

            Assert.AreEqual(expectedContent, actualContent);
        }
 
    }
}
