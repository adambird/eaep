using System;
using System.Text.RegularExpressions;

namespace eaep.servicehost.store
{
    public class QueryParser
    {
        public const string FIELD_VALUE_INDICATOR = ":";

        public static IQueryExpression Parse(string query)
        {
            // first check for boolean operators
            Regex r = new Regex(QueryParser.BooleanOperatorRegEx);
            Match m = r.Match(query);

            if (m.Success)
            {
                BooleanQueryExpression expression = new BooleanQueryExpression()
                {
                    Left = QueryParser.Parse(query.Substring(0, m.Index)),
                    Operator = (BooleanOperator)Enum.Parse(typeof(BooleanOperator), m.Value.Trim()),
                    Right = QueryParser.Parse(query.Substring(m.Index + m.Length))
                };
                return expression;
            }
            else
            {
                return BuildComparisonExpression(query);
            }
        }

        private static IQueryExpression BuildComparisonExpression(string query)
        {
            string[] elements = query.Split(FIELD_VALUE_INDICATOR.ToCharArray(), 2);

            ComparisonQueryExpression expression = new ComparisonQueryExpression()
            {
                Comparator = ExpressionComparator.Equals,
            };

            if (elements.Length == 1)
            {
                expression.Field = null;
                expression.Value = elements[0];
            }
            else
            {
                expression.Field = elements[0];
                expression.Value = elements[1];
            }

            return expression;
        }

        protected static string BooleanOperatorRegEx
        {
            get
            {
                return string.Format(@"(?:\s{0}\s)", string.Join(@"\s|\s", Enum.GetNames(typeof(BooleanOperator))));
            }
        }
    }
}
