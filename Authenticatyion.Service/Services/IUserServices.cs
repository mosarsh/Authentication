using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Service.Services
{
    public interface IUserServices
    {
        int Authenticate(string token);
    }
}
