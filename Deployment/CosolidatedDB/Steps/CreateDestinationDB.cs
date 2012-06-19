using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using CosolidatedDB.Helpers;

namespace CosolidatedDB.Steps
{
    public class CreateDestinationDB : BaseStep
    {
        public CreateDestinationDB(bool runOnce, int stepOrder, StepType type, Stream sourceStream) : base(runOnce, stepOrder, type, sourceStream) { }

        public override void Execute(SqlConnection dbConnection, string destDBName)
        {
            if(CanNotExecute) return;
            
            base.Execute(dbConnection, destDBName);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            string createSQLStatment = "create database " + destDBName;

            string dropDBCommand = "drop database " + destDBName;

            SqlCommand command = dbConnection.CreateCommand();

            if (DBHelper.CheckDatabaseExists(dbConnection, destDBName))
            {
                command.CommandText = dropDBCommand;
                command.ExecuteNonQuery();
            }

            command.CommandText = createSQLStatment;

            command.ExecuteNonQuery();

            dbConnection.Close();
        }
    }
}
