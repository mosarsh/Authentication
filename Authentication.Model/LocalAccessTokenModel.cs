using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Model
{
    public class LocalAccessTokenModel
    {
        public Guid AccessToken { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset ExpirationDateTime { get; set; }
    }
}
