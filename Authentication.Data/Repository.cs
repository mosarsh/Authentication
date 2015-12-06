using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Model;
using System.Web;

namespace Authentication.Data
{
    public class Repository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void RegisterLocalUser(RegisterModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter retval = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            retval.Direction = System.Data.ParameterDirection.ReturnValue;
            parameters.Add(retval);
            parameters.Add(new SqlParameter("@userName", model.UserName));
            parameters.Add(new SqlParameter("@email", model.Email));
            parameters.Add(new SqlParameter("@firstName", model.FirstName));
            parameters.Add(new SqlParameter("@lastName", model.LastName));
            parameters.Add(new SqlParameter("@passwordHash", model.Password));

            var result = DataAccess.Execute("spInsertUpdateUser", parameters, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LocalAccessTokenModel GenerateAccessToken(string username, string password, bool externalUser = false)
        {

            //TODO: Get from database
            int duration = 7; //int.Parse(WebConfigurationManager.AppSettings["TokenExpirationDurationInMinutes"]);

            Guid token = Guid.NewGuid();
            DateTimeOffset createdDateTime = DateTimeOffset.Now;
            DateTimeOffset expirationDateTime = createdDateTime.AddMinutes(duration);

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter retval = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            retval.Direction = System.Data.ParameterDirection.ReturnValue;
            parameters.Add(retval);
            parameters.Add(new SqlParameter("@userName", username));
            parameters.Add(new SqlParameter("@token", token));
            parameters.Add(new SqlParameter("@createdDateTime", createdDateTime));
            parameters.Add(new SqlParameter("@expirationDateTime", expirationDateTime));
            parameters.Add(new SqlParameter("@passwordHash", password));
            parameters.Add(new SqlParameter("@externalUser", externalUser));


            var result = DataAccess.Execute("spInsertUpdateToken", parameters, null);

            return new LocalAccessTokenModel
                        {
                            AccessToken = token,
                            CreatedDateTime = createdDateTime,
                            ExpirationDateTime = expirationDateTime
                        };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RegisterExternalBindingModel FindExternalUser(string provider, string userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter retval = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            retval.Direction = System.Data.ParameterDirection.ReturnValue;
            parameters.Add(retval);
            parameters.Add(new SqlParameter("@provider", provider));
            parameters.Add(new SqlParameter("@userid", userId));

            var result = DataAccess.Execute("spFindExternalUser", parameters, null);

            var model = new RegisterExternalBindingModel
            {
                //TODO
            };

            return model;
        }

        public void RegisterExternalUser(RegisterExternalBindingModel user)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter retval = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            retval.Direction = System.Data.ParameterDirection.ReturnValue;
            parameters.Add(retval);
            parameters.Add(new SqlParameter("@clientId", user.ClientId));
            parameters.Add(new SqlParameter("@externalAccessToken", user.ExternalAccessToken));
            parameters.Add(new SqlParameter("@provider", user.Provider));
            parameters.Add(new SqlParameter("@userId", user.UserId));
            parameters.Add(new SqlParameter("@userName", user.UserName));

            var result = DataAccess.Execute("spRegisterExternalUser", parameters, null);
        }
    }
}
