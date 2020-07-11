using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StudentsMS.Utils
{
    public class SqlPrepareContent
    {
        public string Name { get; set; }
        public SqlDbType Type { get; set; }
        public object Value { get; set; }
        public SqlPrepareContent(string name, SqlDbType type, object value)
        {
            Name = name; Value = value; Type = type;
        }
    }
    public static class SqlHelper
    {

        public static void SqlQueryPrepare(string prepareString, List<SqlPrepareContent> sqlPrepares, Action<SqlDataReader> action)
        {
            using SqlConnection connection = new SqlConnection(AppSettings.SQLConnectString);
            connection.Open();
            SqlCommand command = new SqlCommand(null, connection)
            {
                // Create and prepare an SQL statement.
                CommandText = prepareString
            };
            if (sqlPrepares != null)
                foreach (var item in sqlPrepares)
                {
                    var sp = new SqlParameter(item.Name, item.Type, item.Value.ToString().Length)
                    {
                        Value = item.Value
                    };
                    command.Parameters.Add(sp);
                }
            // Call Prepare after setting the Commandtext and Parameters.
            command.Prepare();
            action(command.ExecuteReader());
        }

        public static int SqlTPrepare(string prepareString, List<SqlPrepareContent> sqlPrepares)
        {
            using SqlConnection connection = new SqlConnection(AppSettings.SQLConnectString);
            connection.Open();
            SqlCommand command = new SqlCommand(null, connection)
            {
                // Create and prepare an SQL statement.
                CommandText = prepareString
            };
            foreach (var item in sqlPrepares)
            {
                var sp = new SqlParameter(item.Name, item.Type, item.Value.ToString().Length)
                {
                    Value = item.Value
                };
                command.Parameters.Add(sp);
            }
            // Call Prepare after setting the Commandtext and Parameters.
            command.Prepare();

            return command.ExecuteNonQuery();
        }
    }
}
