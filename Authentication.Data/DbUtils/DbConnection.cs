using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

namespace Authentication.Data
{
    public class DbConnection
    {
        public static String DefaultConnectionName()
        {
            return WebConfigurationManager.ConnectionStrings["UserSecurityDBContext"].Name;
        }

        public static SqlCommand OpenCommand(String StoredProcedure, List<SqlParameter> sqlParameters, String ConnectionName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName ?? DefaultConnectionName()];

            if (settings == null)
            {
                throw new Exception("No connectionstring found");
            }

            SqlCommand cmd = new SqlCommand(StoredProcedure, new SqlConnection(settings.ConnectionString));

            cmd.CommandType = CommandType.StoredProcedure;

            if (null != sqlParameters)
            {
                CheckParameters(sqlParameters);

                cmd.Parameters.AddRange(sqlParameters.ToArray());
            }
            cmd.CommandTimeout = 60; // 1 minute
            cmd.UpdatedRowSource = UpdateRowSource.None;
            cmd.Connection.Open();
            return cmd;
        }

        private static void CheckParameters(List<SqlParameter> sqlParameters)
        {
            foreach (SqlParameter parm in sqlParameters)
            {
                if (null == parm.Value)
                    parm.Value = DBNull.Value;
            }
        }

        public static void CloseCommand(SqlCommand sqlCommand)
        {
            if (null != sqlCommand && sqlCommand.Connection.State == ConnectionState.Open)
                sqlCommand.Connection.Close();
        }
        
    }
}
