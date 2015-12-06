using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Authentication.Service.Helpers
{
    public class InsertDataHelper
    {
        /// <summary>
        /// Insert the data into database
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="parms">List<SqlParameter></param>
        /// <param name="ConnectionName"></param>
        public static Int32 InsertData(String StoredProcedure, List<SqlParameter> parms, String ConnectionName)
        {
            SqlCommand myCommand = null;
            Int32 result = 0;
            try
            {
                myCommand = DbConnectionHelper.OpenCommand(StoredProcedure, parms, ConnectionName);
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
                DbConnectionHelper.CloseCommand(myCommand);
            }
            return result;
        }
    }
}