using Authentication.Data;
using Authentication.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Authentication.Service.Services
{
    public class UserServices : IUserServices
    {
        public int Authenticate(string token) {

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter retval = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            retval.Direction = System.Data.ParameterDirection.ReturnValue;
            parameters.Add(retval);
            parameters.Add(new SqlParameter("@token", token));
            
            var userId = (int)DataAccess.Execute("spGetUserId", parameters, null);

            if (userId > 0)
            {
                return userId;
            }

            return 0;
        }
    }
}