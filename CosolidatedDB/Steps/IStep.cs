using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Steps
{
    public interface IStep
    {
        void Execute(SqlConnection dbConnection,  string destDBName);
    }
}
