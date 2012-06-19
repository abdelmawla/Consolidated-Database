using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Helpers
{
    public class Statement
    {
        public string TableName { get; set; }
        public string From { get; set; }
        public string Insert { get; set; }
        public string Select { get; set; }
    }
}
