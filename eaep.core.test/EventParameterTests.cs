using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eaep.test
{
    [TestClass]
    public class EventParameterTests
    {
        [TestMethod]
        public void Constructor_WithNullName_ThrowsArgumentNullException()
        {
            try
            {
                new EventParameter(null, "value");
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
                
            }
        }
        [TestMethod]
        public void Constructor_WithNullValue_ThrowsArgumentNullException()
        {
            try
            {
                new EventParameter("name", null);
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {

            }
        }

        [TestMethod]
        public void Constructor_WithEmptyName_ThrowsArgumentException()
        {
            try
            {
                new EventParameter("", "value");
                Assert.Fail();
            }
            catch(ArgumentException)
            {

            }
        }


        [TestMethod]
        public void Constructor_WithEmptyValue_ThrowsArgumentException()
        {
            try
            {
                new EventParameter("name", "");
                Assert.Fail();
            }
            catch(ArgumentException)
            {

            }
        }
    }
}
