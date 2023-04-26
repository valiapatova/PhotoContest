using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.ContestsRepository
{
    public interface IContestsRepository
    {
        Task<List<Contest>> GetAllContests();
        Task<Contest> GetById(int id);
        Task<Contest> GetByTitle(string title);
        Task<Contest> Create(Contest contest);
        Task<Contest> Update(int id, Contest contest);
        Task<Contest> Delete(int id);
        Task<List<Contest>> GetAllContests (ContestQueryParameter filterParameters);
        Task<bool> ContestExists(string title);
        Task<bool> ContestUserExists(int contestId, int userId);

    }
}
