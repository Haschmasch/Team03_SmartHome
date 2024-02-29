using LanguageExt.Common;
using MainUnit.Models.Auth;

namespace MainUnit.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<bool>> RegisterUser(string username, string password);
        public Task<UserLogin> GetUserByName(string username);
    }
}
