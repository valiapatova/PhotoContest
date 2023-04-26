using PhotoContest.Dtos.Contests;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.ContestsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Services.ContestsService
{
    public class ContestsService : IContestsService
    {
        private const string MODIFY_CONTEST_ERROR_MESSAGE = "You can not create, update or delete a contest!";
        private const string VIEW_CONTEST_ERROR_MESSAGE = "You are not authorized to view  contests.";

        private readonly IContestsRepository contestsRepository;
        public ContestsService(IContestsRepository contestsRepository)
        {
            this.contestsRepository = contestsRepository;
        }

        public async Task<List<Contest>> GetAllContests()
        {

            return await this.contestsRepository.GetAllContests();
        }
        public async Task<List<Contest>> GetAllContests(ContestQueryParameter filterParameters)
        {
            return await this.contestsRepository.GetAllContests(filterParameters);
        }

        public async Task<PaginatedList<Contest>> GetAllPaginated(ContestQueryParameter filterParameters)
        {
            List<Contest> contests = await this.contestsRepository.GetAllContests(filterParameters);
            List<Contest> paginatedContests = contests
                                    .Skip((filterParameters.PageNumber - 1) * filterParameters.PageSize)
                                    .Take(filterParameters.PageSize).ToList();
            int totalPages = (contests.Count() + 1) / filterParameters.PageSize;

            return new PaginatedList<Contest>(paginatedContests, totalPages, filterParameters.PageNumber);
        }


        public async Task<List<Contest>> GetAllContests(ContestQueryParameter filterParameters, User userLog)
        {
            if (!(userLog.Role.Name == "junkie" || userLog.Role.Name == "admin"))  
            
            {
                throw new UnauthorizedException(VIEW_CONTEST_ERROR_MESSAGE);
            }
            return await this.contestsRepository.GetAllContests(filterParameters);
        }
        public async Task<Contest> GetById(int id)
        {
            try
            {
                return await this.contestsRepository.GetById(id);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }
        public async Task<Contest> GetByTitle(string title)
        {
            try
            {
                return await this.contestsRepository.GetByTitle(title);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<Contest> Create(Contest contest, User userLog)
        {
            if (userLog.Role.Name != "admin")            
            {
                throw new UnauthorizedException(MODIFY_CONTEST_ERROR_MESSAGE);
            }

            bool duplicateExists = true;

            try
            {
                var contestToCreate= await this.contestsRepository.GetByTitle(contest.Title);

            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Contest with {contest.Title} already exists  ");
            }

            var createdContest = await this.contestsRepository.Create(contest);
            return createdContest;
        }

        public async Task<Contest> Update(int id, Contest contest, User userLog)
        {

            if (!(userLog.Role.Name == "admin"))           
            {
                throw new UnauthorizedException(MODIFY_CONTEST_ERROR_MESSAGE);
            }

            bool duplicateExists = true;
            try
            {
                var existingContest = await this.contestsRepository.GetByTitle(contest.Title);
                if (existingContest.Id == contest.Id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException("Contest Title exists in another Id   ");
            }

            var updatedContest = this.contestsRepository.Update(id, contest);
            return await updatedContest;
        }


        public async Task<Contest> Delete(int id, User userLog)
        {
            if (userLog.Role.Name != "admin")           
            {
                throw new UnauthorizedException(MODIFY_CONTEST_ERROR_MESSAGE);
            }

            try
            {
                return await this.contestsRepository.Delete(id);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<bool> ContestExists(string title)
        {
            return await this.contestsRepository.ContestExists(title);
        }

        public async Task<bool> ContestUserExists(int contestId, int userId)
        {
            return await this.contestsRepository.ContestUserExists(contestId,userId);
        }
    }
}
