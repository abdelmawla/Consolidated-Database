using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CosolidatedDB.Helpers;
using CosolidatedDB.Properties;

namespace CosolidatedDB.Steps
{
    public class CreateRelationShips : BaseStep
    {
        public CreateRelationShips(bool runOnce, int stepOrder, StepType type = StepType.First) : base(runOnce, stepOrder, type) { }

        public override void Execute(SqlConnection dbConnection, string destDBName)
        {
            if (CanNotExecute) return;

            base.Execute(dbConnection, destDBName);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            using (Stream stream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream("CosolidatedDB.Scripts.Relations.txt"))
            {
                if (stream == null) throw new Exception(Resources.FileNotFound);

                dbConnection.ChangeDatabase(destDBName);

                foreach (string commandText in FileHelper.LoadItemsLines(new StreamReader(stream)))
                {
                    try
                    {
                        string tempCommand = commandText.Replace("[NewDataBase]", "[" + destDBName + "]");

                        SqlCommand command = dbConnection.CreateCommand();

                        command.CommandText = tempCommand;

                        command.CommandTimeout = AppConfig.CommandTimeOut;

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(ex);
                    }
                }
            }
        }
    }
}
