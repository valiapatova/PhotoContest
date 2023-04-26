using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Services.UsersService
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters);
        Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters, User userLog);
        Task<User> CreateUser(User user, string password);
        Task<User> GetUserById(int id);
        Task<User> GetUserById(int id, User userLog);
        Task<User> GetUserByUsername(string username);        
        Task<User> DeleteUser(int id, User user);
        Task<User> UpdateUser(int id, UpdateUserDto updateUser, User userLogin, string password);
    }
}
