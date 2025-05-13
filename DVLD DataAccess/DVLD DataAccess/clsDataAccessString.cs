using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;

namespace clsPeopleDataAccess
{
     class clsDataAccessSetting
    {
        public static string StringDataAccess = "Server=.;Database=DVLD;User id=sa;Password=123456";
        // public static string StringDataAccess = ConfigurationManager.ConnectionStrings["stringDataAccess"].ConnectionString;
        Configuration Config;
        public clsDataAccessSetting()
        {
            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        public string GetConnectionString(string StringDataAccess)
        {
            return Config.ConnectionStrings.ConnectionStrings[StringDataAccess].ConnectionString;
        }
    }
}
