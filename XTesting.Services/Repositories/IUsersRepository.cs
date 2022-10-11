using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTesting.Services.Repositories
{
    public interface IUsersRepository
    {
        IQueryable<AppUser> Get();
        AppUser GetByEmail(string email);
        Task<IdentityResult> Create(AppUser user, string password);
        Task<IdentityResult> Delete(AppUser user);
        Task<IdentityResult> Update(AppUser user);
        UserManager<AppUser> GetUserManager();
    }
}
