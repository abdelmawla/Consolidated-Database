using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Helpers
{
    public static class AppConfig
    {
        public static int CommandTimeOut
        {
            get { 
                
                string timeOutValue = ConfigurationManager.AppSettings["CommandTimeOut"];
                
                int result = 18000;

                int.TryParse(timeOutValue, out result);
                
                return result;
            }
        }

        public static string LogFile
        {
            get
            {
                string logFilePath = ConfigurationManager.AppSettings["LogFile"];
                return logFilePath ?? @"c:\log.txt";
            }
        }
    }
}
