using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Interfaces.Services
{
    public interface IUserService
    {
        public Task<LoginResult> SingIn(Login user);
        public Task CreateAccount(Login user);
    }
}
