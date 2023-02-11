using System;
using System.Collections;
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
        public Task ChangeUserRole(NameAndRole user);
        public Task DeleteAccount(NameAndRole user);
        public Task BanAccount(NameAndRole user);
        public Task UnBanAccount(NameAndRole user);
        public Task<IList<NameAndRole>> GetUsersList();
    }
}
