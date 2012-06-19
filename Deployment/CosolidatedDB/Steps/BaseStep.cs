using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Steps
{
    public class BaseStep : IStep
    {
        public int StepOrder { get; set; }
        
        public StepType Type { get; set; }

        public List<Exception> Errors = new List<Exception>();
 
        protected bool RunBefore {get; set;}

        public bool CanNotExecute { get { return RunOnce && RunBefore; }}

        protected Stream SourceStream { get; set; }

        protected BaseStep(bool runOnce, int stepOrder, StepType type, Stream sourceStream = null)
        {
            RunOnce = runOnce;
            StepOrder = stepOrder;
            Type = type;
            SourceStream = sourceStream;
        }

        public bool RunOnce { get; set; }

        public virtual void Execute(SqlConnection dbConnection, string destDBName) 
        {
            RunBefore = true;
        }
    }
}
