using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace clsPeopleDataAccess
{
    public class clsLogEvent
    {
        
       private static string _sourceName = "DVLD";
        ///<summary> 
        ///This Method For Loging Try Catch Exception From Data Access 
        ///</summary>
        ///<param name="Message"></param>
        ///<param name="Type"></param>
        public static void LogExceptionToLogViwer(string Message,EventLogEntryType type)
        {
            if(!EventLog.SourceExists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, "Application");
            }
            EventLog.WriteEntry(_sourceName, Message, type);
        }
    }
}
