using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CosolidatedDB.Helpers
{
    public class Logger
    {
        public static void Log(Exception ex)
        {
            if(ex == null) return;
            
            var exMessage = new StringBuilder();
            
            exMessage.AppendLine(ex.Message);
            exMessage.AppendLine("----------------------------------------");
            
            File.AppendAllText(AppConfig.LogFile, exMessage.ToString());
        }
    }
}
