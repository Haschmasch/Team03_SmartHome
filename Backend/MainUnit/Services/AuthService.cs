using LanguageExt.Common;
using MainUnit.Models.Auth;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Settings;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MainUnit.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<UserLogin> _userLogin;

        public AuthService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _userLogin = mongoDatabase.GetCollection<UserLogin>(
                settings.Value.AuthCollectionName);
        }

        public async Task<Result<bool>> RegisterUser(string username, string password)
        {
            try
            {
                var result = await _userLogin.FindAsync(x => x.Username == username);

                if (await result.AnyAsync())
                    return new Result<bool>(
                        new UserAlreadyExistsException($"User with name '{username}' already exists."));

                await _userLogin.InsertOneAsync(new UserLogin(username, password));
            }
            catch (Exception ex)
            {
                return new Result<bool>(ex);
            }

            return new Result<bool>(true);
        }

        public async Task<UserLogin> GetUserByName(string username)
        {
            try
            {
                var result = await _userLogin.FindAsync(x => x.Username == username);

                return result.First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
