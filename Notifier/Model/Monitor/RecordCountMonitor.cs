using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Notifier.Model.Monitor
{
    public class RecordCountMonitor : OverLimitMonitor
    {
        public RecordCountMonitor()
        {
            Parameters = new Dictionary<string, object>();
            CommandTimeout = 30;
            CommandType = CommandType.Text;
        }

        public int CommandTimeout { get; set; }

        public CommandType CommandType { get; set; }

        public IDbConnection DbConnection { get; set; }

        public Dictionary<string, object> Parameters { get; private set; }

        public string CommandText { get; set; }

        protected override void PerformCheck()
        {
            Debug.Assert(DbConnection != null, "DbConnection is null");
            Debug.Assert(!string.IsNullOrEmpty(CommandText), "QueryCommand should not be empty");

            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();

            try
            {
                IDbCommand command = DbConnection.CreateCommand();
                command.CommandText = CommandText;
                foreach (KeyValuePair<string, object> pair in Parameters)
                {
                    IDbDataParameter parameter = command.CreateParameter();
                    parameter.ParameterName = pair.Key;
                    parameter.Value = pair.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
                Current = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                DbConnection.Close();
            }
        }
    }
}