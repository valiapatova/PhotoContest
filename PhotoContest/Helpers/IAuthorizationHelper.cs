using PhotoContest.Models;
using System.Threading.Tasks;

namespace PhotoContest.Helpers
{
    public interface IAuthorizationHelper
    {
        Task<User> TryGetUser(string username);
        Task<User> TryGetUser(string username, string password);
    }
}
