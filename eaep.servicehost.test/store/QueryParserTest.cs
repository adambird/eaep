using eaep.servicehost.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eaep.servicehost.test.store
{
    /// <summary>
    ///This is a test class for SQLMonitorStoreQueryParserTest and is intended
    ///to contain all SQLMonitorStoreQueryParserTest Unit Tests
    ///</summary>
    [TestClass]
    public class QueryParserTest
    {
        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod]
        public void ParseTest_SingleValue()
        {
            // Arrange
            const string query = "Echo";
            IQueryExpression expected = new ComparisonQueryExpression
                                        {
                Field = null,
                Comparator = ExpressionComparator.Equals,
                Value = query
            };

            // Act
            var actual = QueryParser.Parse(query);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTest_SingleValueSpecifiedField()
        {
            // Arrange
            const string query = "app:Echo";
            IQueryExpression expected = new ComparisonQueryExpression
                                        {
                Field = "app",
                Comparator = ExpressionComparator.Equals,
                Value = "Echo"
            };

            // Act
            var actual = QueryParser.Parse(query);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTest_TwoPartBooleanExpression()
        {
            // Arrange
            const string query = "app:Echo AND host:app1";

            IQueryExpression expected = new BooleanQueryExpression
                                        {
                Left = new ComparisonQueryExpression
                       {
                           Field = "app", 
                           Comparator = ExpressionComparator.Equals, 
                           Value = "Echo"
                       },
                Operator = BooleanOperator.AND,
                Right = new ComparisonQueryExpression
                        {
                            Field = "host", 
                            Comparator = ExpressionComparator.Equals, 
                            Value = "app1"
                        }
            };

            // Act
            var actual = QueryParser.Parse(query);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTest_ThreePartBooleanExpression()
        {
            // Arrange
            const string query = "app:Echo AND host:app1 OR host:app2";

            IQueryExpression expected = new BooleanQueryExpression
                                        {
                Left = new ComparisonQueryExpression { Field = "app", Comparator = ExpressionComparator.Equals, Value = "Echo" },
                Operator = BooleanOperator.AND,
                Right = new BooleanQueryExpression
                        {
                    Left = new ComparisonQueryExpression { Field = "host", Comparator = ExpressionComparator.Equals, Value = "app1" },
                    Operator = BooleanOperator.OR,
                    Right = new ComparisonQueryExpression { Field = "host", Comparator = ExpressionComparator.Equals, Value = "app2" }
                }
            };

            // Act
            IQueryExpression actual = QueryParser.Parse(query);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
