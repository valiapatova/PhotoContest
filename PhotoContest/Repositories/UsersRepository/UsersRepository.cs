using Microsoft.EntityFrameworkCore;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.UsersRepository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext context;

        public UsersRepository(DataContext context)
        {
            this.context = context;
        }

        private IQueryable<User> UserQuery
        {
            get
            {
                return this.context.Users
                    .Include(user => user.Role)
                    .Include(user => user.PhotoPosts)
                        .ThenInclude(post => post.Ratings)
                    .Include(user => user.Ratings)
                    .Include(user => user.Contests)
                        .ThenInclude(contestuser => contestuser.Contest);
            }
        }

        public int UserCount
        {
            get
            {
                return this.context.Users.ToList().Count;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.UserQuery.Where(u => u.IsDeleted != true).ToListAsync();
        }


        public async Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters)
        {
            string username = !string.IsNullOrEmpty(filterParameters.Username) ? filterParameters.Username.ToLowerInvariant() : string.Empty;
            string firstname = !string.IsNullOrEmpty(filterParameters.FirstName) ? filterParameters.FirstName.ToLowerInvariant() : string.Empty;
            string lastname = !string.IsNullOrEmpty(filterParameters.LastName) ? filterParameters.LastName.ToLowerInvariant() : string.Empty;
            string email = !string.IsNullOrEmpty(filterParameters.Email) ? filterParameters.Email.ToLowerInvariant() : string.Empty;

            string sortCriteria = !string.IsNullOrEmpty(filterParameters.SortBy) ? filterParameters.SortBy.ToLowerInvariant() : string.Empty;
            string sortOrder = !string.IsNullOrEmpty(filterParameters.SortOrder) ? filterParameters.SortOrder.ToLowerInvariant() : string.Empty;

            IQueryable<User> result = this.UserQuery;

            result = FilterByFirstName(result, firstname);
            result = FilterByLastName(result, lastname);
            result = FilterByUsername(result, username);
            result = FilterByEmail(result, email);

            result = SortBy(result, sortCriteria);
            result = Order(result, sortOrder);

            return await result.Where(r => r.IsDeleted != true).ToListAsync();
        }

        public async Task<User> CreateUser(User user, string password)
        {
            if (await UserExists(user.Username))
            {
                throw new DuplicateEntityException("User already exists!");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var createdUser = this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
            return createdUser.Entity;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await this.UserQuery.Where(u => u.Id == id && u.IsDeleted != true).FirstOrDefaultAsync();
            return user ?? throw new EntityNotFoundException("User not found!");
        }
        public async Task<User> DeleteUser(int id)
        {
            var userToDelete = await this.GetUserById(id);
            userToDelete.IsDeleted = true;
            await this.context.SaveChangesAsync();
            return userToDelete ?? throw new EntityNotFoundException("User not found!");
        }

        public async Task<User> UpdateUser(int id, UpdateUserDto updateUser, string password)
        {
            var user = await this.GetUserById(id);
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Username = updateUser.Username;
            user.Email = updateUser.Email;

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this.context.SaveChangesAsync();

            return await this.GetUserById(id);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            var user = await this.UserQuery.FirstOrDefaultAsync(u => u.Username == username);          
            return user ?? throw new EntityNotFoundException("User not found!");
        }

        private static IQueryable<User> FilterByFirstName(IQueryable<User> result, string firstname)
        {
            return result.Where(u => u.FirstName.Contains(firstname));
        }

        private static IQueryable<User> FilterByLastName(IQueryable<User> result, string lastname)
        {
            return result.Where(u => u.LastName.Contains(lastname));
        }

        private static IQueryable<User> FilterByEmail(IQueryable<User> result, string email)
        {
            return result.Where(u => u.Email.Contains(email));
        }
        private static IQueryable<User> FilterByUsername(IQueryable<User> result, string username)
        {
            return result.Where(u => u.Username.Contains(username));
        }
        private static IQueryable<User> SortBy(IQueryable<User> result, string sortcriteria)
        {
            switch (sortcriteria)
            {
                case "username":
                    return result.OrderBy(u => u.Username);
                case "firstname":
                    return result.OrderBy(u => u.FirstName);
                case "lastname":
                    return result.OrderBy(u => u.LastName);
                case "email":
                    return result.OrderBy(u => u.Email);

                default:
                    return result;
            }
        }

        private static IQueryable<User> Order(IQueryable<User> result, string sortorder)
        {
            return (sortorder == "desc") ? result.Reverse() : result;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await this.context.Users.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower())))
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
    }
}
