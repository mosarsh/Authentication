using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Authentication.Service.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public int UserId { get; set; }
        public string Token { get; set; }

        public BasicAuthenticationIdentity(string token)
            : base(token, "Basic")
        {
            Token = token;
        }
    }
}
