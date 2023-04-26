using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Repositories.AuthRepository;
using PhotoContest.Services.UsersService;
using System.Threading.Tasks;

namespace PhotoContest.Helpers
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly IUsersService usersService;
        private readonly IAuthRepository authRepository;

        public AuthorizationHelper(IUsersService usersService, IAuthRepository authRepository)
        {
            this.usersService = usersService;
            this.authRepository = authRepository;
        }

        public async Task<User> TryGetUser(string username)
        {
            try
            {
                return await this.usersService.GetUserByUsername(username);
            }
            catch (EntityNotFoundException)
            {
                throw new AuthenticationException("Invalid Username");
            }
        }

        public async Task<User> TryGetUser(string username, string password)
        {
            var user = await this.TryGetUser(username);
            bool IsPasswordValid = this.authRepository.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

            if (!IsPasswordValid)
            {
                throw new AuthenticationException("Authentication failed. Check credentials.");
            }            
            return user;
        }

    }
}
