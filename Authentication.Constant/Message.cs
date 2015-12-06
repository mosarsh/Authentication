using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Constant
{
    public class Message
    {
        public const string UserRegistrationSucceess = "User has been successfully registered";
        public const string RequiredRedirectURI = "redirect_uri is required";
        public const string InvalidRedirectURI = "redirect_uri is invalid";
        public const string RequiredClientId = "client_Id is required";
    }
}
