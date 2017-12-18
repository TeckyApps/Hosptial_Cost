using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class SqlConnect
    {
        // Method for returning the SQL Connection String from the web.config
        public string GetSqlConnection_String()
        {
            return ConfigurationManager.ConnectionStrings["SQL_Connection"].ConnectionString;
        }

    }
}