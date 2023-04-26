using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Models.QueryParameters;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.UsersRepository
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters);
        Task<User> CreateUser(User user, string password);
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);        
        Task<User> DeleteUser(int id);
        Task<User> UpdateUser(int id, UpdateUserDto updateUser, string password);
    }
}
