using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTesting.Services.Models;

namespace XTesting.Services.Services
{
    public interface IUsersService
    {
        IQueryable<UserModel> Get();
        UserModel GetByEmail(string email);
        Task<IdentityResult> Create(UserModel user, string password);
        Task<IdentityResult> Delete(UserModel user);
        Task<IdentityResult> Update(UserModel user);
        Task<IdentityResult> ValidatePassword(UserModel user, string password);
        Task<IdentityResult> ValidateUser(UserModel user);
        string HashPassword(UserModel user, string password);
        Task SignOutAsync();
        Task<SignInResult> PasswordSignInAsync(UserModel user, string password, bool lockoutOnFailure,
            bool isPersistent);
    }
}
