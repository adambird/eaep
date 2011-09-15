using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eaep.servicehost.store;

namespace eaep.servicehost.test.store
{
    /// <summary>
    ///This is a test class for SQLMonitorStoreHelperTest and is intended
    ///to contain all SQLMonitorStoreHelperTest Unit Tests
    ///</summary>
    [TestClass]
    public class SQLMonitorStoreHelperTest
    {
        /// <summary>
        ///A test for TranslateQueryExpressionToWhereClause
        ///</summary>
        [TestMethod]
        public void TranslateQueryExpressionToWhereClauseTest_SingleFieldSpecified()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression
                                          {
                                              Field = "app", 
                                              Comparator = ExpressionComparator.Equals, 
                                              Value = "Echo"
                                          };
            const string expected = "[f0].[Value] = @vf0 AND [f0].[Field] = @ff0";
            var parameters = new Dictionary<string, object>();
            var fieldTableAliases = new List<string>();

            // Act
            var actual = SQLMonitorStoreHelper.TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("app", parameters["@ff0"]);
            Assert.AreEqual("Echo", parameters["@vf0"]);

        }

        [TestMethod]
        public void TranslateQueryExpressionToWhereClauseTest_NoFieldSpecified()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression
                                          {
                                              Field = null, 
                                              Comparator = ExpressionComparator.Equals, 
                                              Value = "Echo"
                                          };
            const string expected = "[f0].[Value] = @vf0";
            var parameters = new Dictionary<string, object>();
            var fieldTableAliases = new List<string>();

            // Act
            var actual = SQLMonitorStoreHelper.TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("Echo", parameters["@vf0"]);
        }

        [TestMethod]
        public void TranslateQueryExpressionToWhereClauseTest_TwoPartBooleanExpresion()
        {
            // Arrange
            IQueryExpression expression = new BooleanQueryExpression
                                          {
                Left = new ComparisonQueryExpression { Field = "app", Comparator = ExpressionComparator.Equals, Value = "Echo" },
                Operator = BooleanOperator.AND,
                Right = new ComparisonQueryExpression { Field = "host", Comparator = ExpressionComparator.Equals, Value = "app2" }
            };

            const string expected = "([f0].[Value] = @vf0 AND [f0].[Field] = @ff0)" +
                                    " AND ([f1].[Value] = @vf1 AND [f1].[Field] = @ff1)";

            var parameters = new Dictionary<string, object>();
            var fieldTableAliases = new List<string>();

            // Act
            var actual = SQLMonitorStoreHelper.TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("app", parameters["@ff0"]);
            Assert.AreEqual("Echo", parameters["@vf0"]);
            Assert.AreEqual("host", parameters["@ff1"]);
            Assert.AreEqual("app2", parameters["@vf1"]);
        }

        [TestMethod]
        public void GetMessagesSQLCommandTest_SingleExpression()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression
                                          {
                                              Field = "app", 
                                              Comparator = ExpressionComparator.Equals, 
                                              Value = "Echo"
                                          };

            // Act
            var actual = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression);
            
            // Assert
            Assert.AreEqual(2, actual.Parameters.Count);
            Assert.AreEqual("@ff0", actual.Parameters[1].ParameterName);
            Assert.AreEqual("app", actual.Parameters[1].Value);
            Assert.AreEqual("@vf0", actual.Parameters[0].ParameterName);
            Assert.AreEqual("Echo", actual.Parameters[0].Value);

        }

        [TestMethod]
        public void GetMessagesSQLCommandTest_SingleExpressionUnspecifiedField()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression
                                          {
                                              Field = null, 
                                              Comparator = ExpressionComparator.Equals, 
                                              Value = "Echo"
                                          };
            // Act
            var actual = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression);

            // Assert
            Assert.AreEqual(1, actual.Parameters.Count);
            Assert.AreEqual("@vf0", actual.Parameters[0].ParameterName);
            Assert.AreEqual("Echo", actual.Parameters[0].Value);

        }

        [TestMethod]
        public void GetMessagesSQLCommandTest_SingleExpression_Since()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression
                                          {
                                              Field = "app", 
                                              Comparator = ExpressionComparator.Equals, 
                                              Value = "Echo"
                                          };

            var since = DateTime.Now;

            // Act
            var actual = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression, since);

            // Assert
            Assert.AreEqual(3, actual.Parameters.Count);
            Assert.AreEqual("app", actual.Parameters["@ff0"].Value);
            Assert.AreEqual("Echo", actual.Parameters["@vf0"].Value);
            Assert.AreEqual(since, actual.Parameters["@fromTimestamp"].Value);

        }

        [TestMethod]
        public void GetMessagesSQLCommandTest_SingleExpression_FromTo()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression { Field = "app", Comparator = ExpressionComparator.Equals, Value = "Echo" };

            var from = DateTime.Now;
            var to = DateTime.Now.AddDays(1);

            // Act
            var actual = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression, from, to);

            // Assert
            Assert.AreEqual(4, actual.Parameters.Count);
            Assert.AreEqual("app", actual.Parameters["@ff0"].Value);
            Assert.AreEqual("Echo", actual.Parameters["@vf0"].Value);
            Assert.AreEqual(from, actual.Parameters["@fromTimestamp"].Value);
            Assert.AreEqual(to, actual.Parameters["@toTimestamp"].Value);
        }

        [TestMethod]
        [Ignore]
        public void GetMessagesSQLCommandTest_MultipartBooleanExpression()
        {
            // Arrange
            IQueryExpression expression = new BooleanQueryExpression
                                          {
                Left = new ComparisonQueryExpression
                       {
                           Field = "app", 
                           Comparator = ExpressionComparator.Equals, 
                           Value = "Echo"
                       },
                Operator = BooleanOperator.AND,
                Right = new BooleanQueryExpression
                        {
                    Left = new ComparisonQueryExpression
                           {
                               Field = "host", 
                               Comparator = ExpressionComparator.Equals, 
                               Value = "app1"
                           },
                    Operator = BooleanOperator.OR,
                    Right = new ComparisonQueryExpression
                            {
                                Field = "host", 
                                Comparator = ExpressionComparator.Equals, 
                                Value = "app2"
                            }
                }
            };

            // Act
            var actual = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression);

            // Assert
            Assert.AreEqual(2, actual.Parameters.Count);
            Assert.AreEqual("@field1", actual.Parameters[0].ParameterName);
            Assert.AreEqual("app", actual.Parameters[0].Value);
            Assert.AreEqual("@value1", actual.Parameters[1].ParameterName);
            Assert.AreEqual("Echo", actual.Parameters[1].Value);

        }

        [TestMethod]
        public void GetDistinctSQLCommandTest_SingleField()
        {
            // Arrange
            IQueryExpression expression = new ComparisonQueryExpression { Field = "app", Comparator = ExpressionComparator.Equals, Value = "Echo" };

            const string field = "user";
            var from = DateTime.Now;
            var to = DateTime.Now.AddDays(1);

            // Act
            var actual = SQLMonitorStoreHelper.GetDistinctSQLCommand(expression, from, to, field);

            // Assert
            Assert.AreEqual(5, actual.Parameters.Count);
            Assert.AreEqual(field, actual.Parameters["@fd0"].Value);
            Assert.AreEqual("app", actual.Parameters["@ff0"].Value);
            Assert.AreEqual("Echo", actual.Parameters["@vf0"].Value);
            Assert.AreEqual(from, actual.Parameters["@fromTimestamp"].Value);
            Assert.AreEqual(to, actual.Parameters["@toTimestamp"].Value);

        }
    }
}
