using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Authentication.Data
{
    public class DataAccess
    {
        /// <summary>
        /// Insert the data into database
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="parms">List<SqlParameter></param>
        /// <param name="ConnectionName"></param>
        public static Int32 Execute(String StoredProcedure, List<SqlParameter> parms, String ConnectionName)
        {
            SqlCommand myCommand = null;
            Int32 result = 0;
            try
            {
                myCommand = DbConnection.OpenCommand(StoredProcedure, parms, ConnectionName);
                myCommand.ExecuteNonQuery();
                result = (Int32)myCommand.Parameters["@RETURN_VALUE"].Value;
            }
            catch (Exception err)
            {
                // do something with the error
                throw err;
            }
            finally
            {
                DbConnection.CloseCommand(myCommand);
            }
            return result;
        }
    }
}