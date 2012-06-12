using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Helpers
{
    public class DBHelper
    {
        public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            bool result;

            try
            {
                string sqlCreateDBQuery = String.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                {
                    var databaseID = (int)sqlCmd.ExecuteScalar();

                    result = (databaseID > 0);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static bool CheckTableExists(SqlConnection dbConnection, string tableName)
        {
            try
            {
                string sqlCreateDBQuery = String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='{0}'", tableName);

                using (var sqlCmd = new SqlCommand(sqlCreateDBQuery, dbConnection))
                {
                    return sqlCmd.ExecuteScalar() != null;
                }
            }
            catch { return false; }

        }

        public static Statement ParseCommand(string command)
        {
            string[] insetIntopart = command.ToLower().Split(new string[] { " into " }, StringSplitOptions.RemoveEmptyEntries);

            string selectStatmentWithoutKeywordSelect = insetIntopart[0].Remove(0, 7).Trim();

            string[] fromPart = insetIntopart[1].ToLower().Split(new string[] { " from " }, StringSplitOptions.RemoveEmptyEntries);

            string insertInto = fromPart[0].Trim();

            string from = fromPart[1].Trim();

            string[] tableNameParts = insertInto.Split(new char[] { '.' });

            string tableName = tableNameParts[2].Trim().Replace("[", string.Empty).Replace("]", string.Empty);

            return new Statement { TableName = tableName, From = from, Select = selectStatmentWithoutKeywordSelect, Insert = insertInto };
        }
    }
}
