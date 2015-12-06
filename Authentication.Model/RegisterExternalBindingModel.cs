using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authentication.Model
{
    public class RegisterExternalBindingModel
    {
        public string UserName { get; set; }
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
        public string UserId { get; set; }
        public string ClientId { get; set; }
    }
}
