using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace eaep.servicehost.store
{
    public class SQLMonitorStore : IEAEPMonitorStore
    {
        protected static ILog log = LogManager.GetLogger(typeof(SQLMonitorStore));

        #region IEAEPMonitorStore Members

        protected const string PUSH_MESSAGE_SQL = "INSERT Messages(Message, Timestamp) VALUES(@message, @timestamp)\r\n"
                + "set @id = @@IDENTITY";

        protected const string PUSH_FIELD_SQL = "INSERT Fields(MessageID, Field, Value) VALUES(@messageID, @field, @value)";

        protected string connectionString;

        public void Close()
        {
        }

        public SQLMonitorStore(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            
            connection.Open();

            return connection;
        }

        public EAEPMessages GetMessages(string query)
        {
            IQueryExpression expression = QueryParser.Parse(query);

            using (SqlCommand command = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression))
            {
                return ExecuteGetMessagesCommand(command);
            }
        }

        public EAEPMessages GetMessages(DateTime since, string query)
        {
            IQueryExpression expression = QueryParser.Parse(query);

            using (SqlCommand command = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression, since))
            {
                return ExecuteGetMessagesCommand(command);
            }
        }

        public EAEPMessages GetMessages(DateTime from, DateTime to, string query)
        {
            IQueryExpression expression = QueryParser.Parse(query);

            using (SqlCommand command = SQLMonitorStoreHelper.GetMessagesSQLCommand(expression, from, to))
            {
                return ExecuteGetMessagesCommand(command);
            }
        }

        protected EAEPMessages ExecuteGetMessagesCommand(SqlCommand command)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    command.Connection = connection;
                    command.CommandTimeout = Configuration.DefaultQueryTimeout;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        EAEPMessages messages = new EAEPMessages();

                        while (reader.Read())
                        {
                            messages.Add(new EAEPMessage(reader.GetString(0)));
                        }

                        return messages;
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Warn(ex);
                throw new MessageRetrievalException(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MessageRetrievalException(ex.Message);
            }

        }

        public string[] Distinct(string field, DateTime from, DateTime to, string query)
        {
            try
            {
                IQueryExpression expression = QueryParser.Parse(query);

                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = SQLMonitorStoreHelper.GetDistinctSQLCommand(expression, from, to, field))
                    {
                        command.Connection = connection;
                        command.CommandTimeout = Configuration.DefaultQueryTimeout;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<string> values = new List<string>();

                            while (reader.Read())
                            {
                                values.Add(reader.GetString(0));
                            }

                            return values.ToArray();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Warn(ex);
                throw new MessageRetrievalException(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MessageRetrievalException(ex.Message);
            }
        }


        public void PushMessage(EAEPMessage message)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    log.Debug("Initiated Push Message transaction");

                    try
                    {
                        long messageID = 0;

                        using (SqlCommand pushMessageCommand = new SqlCommand(PUSH_MESSAGE_SQL, connection, transaction))
                        {
                            pushMessageCommand.CommandTimeout = Configuration.DefaultQueryTimeout;

                            pushMessageCommand.Parameters.AddWithValue("@message", message.ToString());
                            pushMessageCommand.Parameters.AddWithValue("@timestamp", message.TimeStamp);
                            SqlParameter idParam = pushMessageCommand.Parameters.Add("@id", SqlDbType.BigInt);
                            idParam.Direction = ParameterDirection.Output;

                            pushMessageCommand.ExecuteNonQuery();

                            messageID = (long)idParam.Value;
                        }

                        using (SqlCommand pushFieldCommand = new SqlCommand(PUSH_FIELD_SQL, connection, transaction))
                        {
                            pushFieldCommand.CommandTimeout = Configuration.DefaultQueryTimeout;

                            pushFieldCommand.Parameters.Add("@messageID", SqlDbType.BigInt);
                            pushFieldCommand.Parameters.Add("@field", SqlDbType.NVarChar);
                            pushFieldCommand.Parameters.Add("@value", SqlDbType.NVarChar);

                            PushField(pushFieldCommand, messageID, EAEPMessage.FIELD_APPLICATION, message.Application);
                            PushField(pushFieldCommand, messageID, EAEPMessage.FIELD_HOST, message.Host);
                            PushField(pushFieldCommand, messageID, EAEPMessage.FIELD_EVENT, message.Event);

                            foreach (string field in message.ParameterKeys)
                            {
                                PushField(pushFieldCommand, messageID, field, message[field]);
                            }
                        }

                        transaction.Commit();
                        log.Debug("Committed Push Message transaction");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.Error("Error persisting message", ex);
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Warn(ex);
                throw new MessagePersistanceException(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MessagePersistanceException(ex.Message);
            }
        }

        private static void PushField(SqlCommand pushFieldCommand, long messageID, string field, string value)
        {
            pushFieldCommand.Parameters["@messageID"].Value = messageID;
            pushFieldCommand.Parameters["@field"].Value = field;
            pushFieldCommand.Parameters["@value"].Value = value;

            pushFieldCommand.ExecuteNonQuery();
        }

        public void PushMessages(EAEPMessages messages)
        {
            foreach (EAEPMessage message in messages)
            {
                PushMessage(message);
            }
        }


        public CountResult[] Count(DateTime from, DateTime to, int timeSlices, string query)
        {
            return Count(from, to, timeSlices, null, query);
        }

        public CountResult[] Count(DateTime from, DateTime to, int timeSlices, string field, string query)
        {
            try
            {
                IQueryExpression expression = QueryParser.Parse(query);

                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = SQLMonitorStoreHelper.GetCountSQLCommand(expression, from, to, timeSlices, field))
                    {
                        command.Connection = connection;
                        command.CommandTimeout = Configuration.DefaultQueryTimeout;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<CountResult> results = new List<CountResult>();

                            while (reader.Read())
                            {
                                CountResult result = new CountResult()
                                {
                                    TimeSlice = (int)reader["TimeSlice"], Count = (int)reader["Count"]
                                };
                                if (field != null)
                                {
                                    result.FieldValue = (string)reader["Value"];
                                }
                                results.Add(result);
                            }

                            return results.ToArray();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Warn(ex);
                throw new MessageRetrievalException(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MessageRetrievalException(ex.Message);
            }
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
