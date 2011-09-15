using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using log4net;

namespace eaep.servicehost.store
{
    public class SQLMonitorStoreHelper
    {
        static ILog log = LogManager.GetLogger(typeof(SQLMonitorStoreHelper));

        protected const string GET_MESSAGES_SQL_BASE = "SELECT DISTINCT TOP 1000 [Messages].[Message], [Messages].[ID] FROM [Messages] (NOLOCK)";

        protected const string GET_MESSAGES_SQL_ORDER_BY = "ORDER BY [Messages].[ID] DESC";

        public static SqlCommand GetMessagesSQLCommand(IQueryExpression expression)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            List<string> fieldTableAliases = new List<string>();

            SqlCommand command = GetCommand(TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases), fieldTableAliases);

            LoadParameterValues(command, parameters);

            return command;
        }

        public static SqlCommand GetMessagesSQLCommand(IQueryExpression expression, DateTime since)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            List<string> fieldTableAliases = new List<string>();

            parameters.Add("@fromTimestamp", since);

            sql.Append("[Messages].[Timestamp] > @fromTimestamp");
            sql.Append(" AND ");

            sql.Append(TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases));

            SqlCommand command = GetCommand(sql.ToString(), fieldTableAliases);

            LoadParameterValues(command, parameters);

            return command;

        }

        public static SqlCommand GetMessagesSQLCommand(IQueryExpression expression, DateTime from, DateTime to)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            List<string> fieldTableAliases = new List<string>();

            AppendFromToParameters(from, to, sql, parameters);

            sql.Append(TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases));

            SqlCommand command = GetCommand(sql.ToString(), fieldTableAliases);

            LoadParameterValues(command, parameters);

            return command;

        }

        private static void AppendFromToParameters(DateTime from, DateTime to, StringBuilder sql, Dictionary<string, object> parameters)
        {
            parameters.Add("@fromTimestamp", from);
            parameters.Add("@toTimestamp", to);

            sql.Append("[Messages].[Timestamp] BETWEEN @fromTimestamp AND @toTimestamp");
            sql.Append(" AND ");
        }

        public static SqlCommand GetDistinctSQLCommand(IQueryExpression expression, DateTime from, DateTime to, string field)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            List<string> fieldTableAliases = new List<string>();

            AppendFromToParameters(from, to, sql, parameters);

            sql.Append(TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases));

            SqlCommand command = GetCommand(
                "SELECT fd.Value, MAX([Messages].[Timestamp]) [Latest] FROM [Messages] (NOLOCK) INNER JOIN [Fields] fd (NOLOCK) ON Messages.ID = fd.MessageID AND fd.Field = @fd0", 
                sql.ToString(),
                "GROUP BY fd.[Value] ORDER BY [Latest] DESC", 
                fieldTableAliases);

            parameters.Add("@fd0", field);

            LoadParameterValues(command, parameters);

            return command;
        }

        public static SqlCommand GetCountSQLCommand(IQueryExpression expression, DateTime from, DateTime to, int timeSlices, string field)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            List<string> fieldTableAliases = new List<string>();

            StringBuilder sqlSelect = new StringBuilder();
            sqlSelect.AppendLine("declare @timeSliceDuration int");
            sqlSelect.AppendLine("set @timeSliceDuration = DATEDIFF(s, @fromTimestamp, @toTimestamp) / @timeSlices");
            sqlSelect.Append("select DATEDIFF(s, @fromTimestamp, [Messages].[Timestamp]) / @timeSliceDuration AS TimeSlice");
            if (field != null)
            {
                sqlSelect.Append(", fd.[Value]");
            }
            sqlSelect.Append(", COUNT(*) [Count] FROM [Messages] (NOLOCK) ");
            if (field != null)
            {
                sqlSelect.Append("INNER JOIN [Fields] fd (NOLOCK) ON Messages.ID = fd.MessageID AND fd.Field = @fd0");
                parameters.Add("@fd0", field);
            }

            StringBuilder sqlOrderBy = new StringBuilder();
            sqlOrderBy.Append("GROUP BY DATEDIFF(s, @fromTimestamp, [Messages].[Timestamp]) / @timeSliceDuration");
            if (field != null)
            {
                sqlOrderBy.Append(", fd.[Value]");
            }
            sqlOrderBy.Append("ORDER BY [TimeSlice] ASC");

            AppendFromToParameters(from, to, sql, parameters);

            sql.Append(TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases));

            SqlCommand command = GetCommand(
                sqlSelect.ToString(),
                sql.ToString(),
                sqlOrderBy.ToString(),
                fieldTableAliases);

            parameters.Add("@timeSlices", timeSlices);

            LoadParameterValues(command, parameters);

            return command;
        }

        public static SqlCommand GetCommand(string whereClause, List<string> fieldTableAliases)
        {
            return GetCommand(GET_MESSAGES_SQL_BASE, whereClause, GET_MESSAGES_SQL_ORDER_BY, fieldTableAliases);
        }
        
        public static SqlCommand GetCommand(string selectBase, string whereClause, string orderByClause, List<string> fieldTableAliases)
        {
            StringBuilder sqlStatement = new StringBuilder(selectBase);

            foreach (string alias in fieldTableAliases)
            {
                sqlStatement.AppendFormat(" INNER JOIN [Fields] {0} (NOLOCK) ON [Messages].[ID] = [{0}].[MessageID]", alias);
            }

            sqlStatement.Append(" WHERE ");
            sqlStatement.Append(whereClause);
            sqlStatement.Append(" ");

            sqlStatement.Append(orderByClause);

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("SQLCommand generated: {0}", sqlStatement.ToString());
            }

            return new SqlCommand(sqlStatement.ToString());
        }

        public static string TranslateQueryExpressionToWhereClause(IQueryExpression expression, Dictionary<string, object> parameters, List<string> fieldTableAliases)
        {
            return TranslateQueryExpressionToWhereClause(expression, parameters, fieldTableAliases, new StringBuilder());
        }

        public static string TranslateQueryExpressionToWhereClause(IQueryExpression expression, Dictionary<string, object> parameters, List<string> fieldTableAliases, StringBuilder sqlStatement)
        {
            if (expression is ComparisonQueryExpression)
            {
                string fieldTableAlias = string.Format("f{0}", fieldTableAliases.Count);

                string param = "@v" + fieldTableAlias;
                sqlStatement.AppendFormat("[{0}].[Value] = {1}", fieldTableAlias, param);
                parameters.Add(param, ((ComparisonQueryExpression)expression).Value);

                if (((ComparisonQueryExpression)expression).Field != null)
                {
                    param = "@f" + fieldTableAlias;

                    sqlStatement.AppendFormat(" AND [{0}].[Field] = {1}", fieldTableAlias, param);
                    parameters.Add(param, ((ComparisonQueryExpression)expression).Field);
                }

                fieldTableAliases.Add(fieldTableAlias);
            }
            else if (expression is BooleanQueryExpression)
            {
                sqlStatement.Append("(");
                sqlStatement.Append(TranslateQueryExpressionToWhereClause(((BooleanQueryExpression)expression).Left, parameters, fieldTableAliases));
                sqlStatement.AppendFormat(") {0} (", ((BooleanQueryExpression)expression).Operator);
                sqlStatement.Append(TranslateQueryExpressionToWhereClause(((BooleanQueryExpression)expression).Right, parameters, fieldTableAliases));
                sqlStatement.Append(")");
            }
            return sqlStatement.ToString();
        }

        protected static void LoadParameterValues(SqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (string key in parameters.Keys)
            {
                command.Parameters.AddWithValue(key, parameters[key]);
            }
        }

    }
}
