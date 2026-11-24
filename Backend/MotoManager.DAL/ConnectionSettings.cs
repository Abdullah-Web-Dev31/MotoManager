using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoManager.DAL
{
    public static class ConnectionSettings
    {

        public static  string  ConnectionString = string.Empty;


        public static SqlConnection _GetConnection()=>new SqlConnection(ConnectionString);
    }
}
