using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using CosolidatedDB.Helpers;
using CosolidatedDB.Properties;

namespace CosolidatedDB.Steps
{
    public class CreateTables : BaseStep
    {
        public CreateTables(bool runOnce, int stepOrder, StepType type, Stream sourceStream) : base(runOnce, stepOrder, type, sourceStream) { }

        public override void Execute(SqlConnection dbConnection, string destDBName)
        {
            if(CanNotExecute) return;

            base.Execute(dbConnection, destDBName);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            
            string currentDirectory  =  Directory.GetCurrentDirectory();

            if (SourceStream == null) SourceStream = File.OpenRead(currentDirectory + "\\Scripts\\Tables.txt");

            using (SourceStream)
            {
                if (SourceStream == null) throw new Exception(Resources.FileNotFound);

                foreach (string commandText in FileHelper.LoadItemsLines(new StreamReader(SourceStream)))
                {
                    try
                    {
                        string tempCommand = commandText.Replace("[NewDataBase]", "[" + destDBName + "]");

                        Statement statement = DBHelper.ParseCommand(tempCommand);

                        SqlCommand command = dbConnection.CreateCommand();

                        command.CommandTimeout = AppConfig.CommandTimeOut;

                        string prevDBName = dbConnection.Database;

                        dbConnection.ChangeDatabase(destDBName);

                        command.CommandText = CreateCommandText(dbConnection, statement);

                        dbConnection.ChangeDatabase(prevDBName);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(ex);
                    }
                }
            }

            dbConnection.Close();
        }

        private static string CreateCommandText(SqlConnection dbConnection, Statement statement)
        {
            return DBHelper.CheckTableExists(dbConnection, statement.TableName) ? 
                string.Format("insert into {0} select {1} from {2}", statement.Insert, statement.Select, statement.From) :
                string.Format("select {0} into {1} from {2}", statement.Select, statement.Insert, statement.From);
        }
    }
}
