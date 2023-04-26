using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Services.ContestsService
{
    public interface IContestsService
    {
        Task<List<Contest>> GetAllContests();
        Task<List<Contest>> GetAllContests(ContestQueryParameter filterParameters);
        Task<PaginatedList<Contest>> GetAllPaginated(ContestQueryParameter filterParameters);
        Task<List<Contest>> GetAllContests(ContestQueryParameter filterParameters, User userLog);
        Task<Contest> GetById(int id);
        Task<Contest> GetByTitle(string title);
        Task<Contest> Create(Contest contest, User userLog);
        Task<Contest> Update(int id, Contest contest, User userLog);
        Task<Contest> Delete(int id, User userLog);
        Task<bool> ContestExists(string title);
        Task<bool> ContestUserExists(int contestId, int userId);
    }
}
