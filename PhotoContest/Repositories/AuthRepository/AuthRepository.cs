using Microsoft.EntityFrameworkCore;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;
        }
        private IQueryable<User> UserQuery
        {
            get
            {
                return this.context.Users
                    .Include(u => u.Role);

            }
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await this.UserQuery.FirstOrDefaultAsync(u=>u.Username.ToLower().Equals(username.ToLower()));

            if(user == null)
            {
                throw new EntityNotFoundException("User not found!");
            }
            else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AuthenticationException("Wrong password!");
            }

            return user;

        }

        public async Task<User> Register(User user, string password)
        {
            if(await UserExists(user.Username))
            {
                throw new DuplicateEntityException("User already exists!");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RoleId = 2;

            this.context.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await this.context.Users.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }


            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) 
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
