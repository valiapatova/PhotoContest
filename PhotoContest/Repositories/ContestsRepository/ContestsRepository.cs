using Microsoft.EntityFrameworkCore;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.Enums;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.PhotoPostsRepository;
using PhotoContest.Repositories.RatingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.ContestsRepository
{
    public class ContestsRepository : IContestsRepository
    {
        private readonly DataContext context;
        private readonly IPhotoPostsRepository photoPostsRepository;
        private readonly IRatingsRepository ratingsRepository;

        public ContestsRepository(DataContext context,
            IPhotoPostsRepository photoPostsRepository, IRatingsRepository ratingsRepository)
        {
            this.context = context;
            this.photoPostsRepository = photoPostsRepository;
            this.ratingsRepository = ratingsRepository;
        }

        private IQueryable<Contest> ContestQuery
        {
            get
            {
                return this.context.Contests.Where(contest => contest.IsDeleted != true)
                    .Include(contest => contest.Category)
                    .Include(contest => contest.PhotoPosts)
                    .ThenInclude(photoPost => photoPost.User) // add Valia
                       .ThenInclude(user => user.Role)        // add Valia
                                                              //   .ThenInclude(photoPost => photoPost.)
                                                              //.ThenInclude(photoPost => photoPost.User) // add Valia
                                                              //   .ThenInclude(user => user.Role)        // add Valia
                    .Include(contest => contest.Users)
                         .ThenInclude(user => user.User)
                            .ThenInclude(user => user.Role);                   
            }
        }

        // GET ContestUsers
        private IQueryable<ContestUser> ContestUserQuery
        {
            get
            {
                return this.context.ContestUser.Where(contestUser => contestUser.IsDeleted != true);
            }
        }

        public async Task<List<Contest>> GetAllContests()
        {
            return await this.ContestQuery.ToListAsync();
        }

        public async Task<Contest> GetById(int id)
        {
            var contest = await this.ContestQuery.Where(contest => contest.Id == id).FirstOrDefaultAsync();  
            return contest ?? throw new EntityNotFoundException("Contest not found!");
        }

        public async Task<Contest> GetByTitle(string title)
        {
            var contest = await this.ContestQuery.Where(contest => contest.Title == title).FirstOrDefaultAsync(); 
            
            return contest ?? throw new EntityNotFoundException($"Contest with title {title} not found!");
        }
        public async Task<Contest> Create(Contest contest)
        {
            var createdContest = context.Contests.Add(contest);
            await context.SaveChangesAsync();

            return createdContest.Entity;
        }

        public async Task<Contest> Update(int id, Contest contest)
        {
            var contestToUpdate = await this.GetById(id);
            contestToUpdate.Title = contest.Title;

            contestToUpdate.CategoryId = contest.CategoryId;

            contestToUpdate.Phase1Start = contest.Phase1Start;
            contestToUpdate.Phase2Start = contest.Phase2Start;
            contestToUpdate.EndDate = contest.EndDate;
            contestToUpdate.PhaseName = contest.PhaseName;
            contestToUpdate.IsOpen = contest.IsOpen;
            contestToUpdate.Users = contest.Users;

            await context.SaveChangesAsync();

            return await this.GetById(id);                          
        }

        public async Task<Contest> Delete(int id)
        {
            var contest = await GetById(id);
            contest.IsDeleted = true;

            List<PhotoPost> photoPosts = await photoPostsRepository.GetByContestId(id);
            foreach (var photoPost in photoPosts)
            {
                await this.photoPostsRepository.DeletePhotoPost(photoPost.Id);
            }

            List<ContestUser> contestUsers = await GetContestUserByContestId(id);
            foreach (ContestUser contestUser in contestUsers)
            {
                contestUser.IsDeleted = true;
            }

            var updatedContest = context.Update(contest);
            await context.SaveChangesAsync();
            return updatedContest.Entity;
        }



        public async Task<List<Contest>> GetAllContests(ContestQueryParameter filterParameters)
        {          
            string title = !string.IsNullOrEmpty(filterParameters.Title) ? filterParameters.Title.ToLowerInvariant() : string.Empty;
            string category = !string.IsNullOrEmpty(filterParameters.Category) ? filterParameters.Category.ToLowerInvariant() : string.Empty;
            string phase = !string.IsNullOrEmpty(filterParameters.Phase) ? filterParameters.Phase.ToLowerInvariant() : string.Empty;
            string type = !string.IsNullOrEmpty(filterParameters.Type) ? filterParameters.Type.ToLowerInvariant() : string.Empty;

            string sortCriteria = !string.IsNullOrEmpty(filterParameters.SortBy) ? filterParameters.SortBy.ToLowerInvariant() : string.Empty;
            string sortOrder = !string.IsNullOrEmpty(filterParameters.SortOrder) ? filterParameters.SortOrder.ToLowerInvariant() : string.Empty;

            IQueryable<Contest> result = this.ContestQuery;

            result = FilterByTitle(result, title);
            result = FilterByCategory(result, category);
            result = FilterByPhase(result, phase);
            result = FilterByType(result, type);

            result = SortBy(result, sortCriteria);
            result = Order(result, sortOrder);

           return await result.Where(r => r.IsDeleted != true).ToListAsync();
        }

        private static IQueryable<Contest> FilterByTitle(IQueryable<Contest> result, string title)
        {
            return result.Where(contest => contest.Title.Contains(title));
        }

        private static IQueryable<Contest> FilterByCategory(IQueryable<Contest> result, string category)
        {
            return result.Where(contest => contest.Category.Name.Contains(category));
        }

        private static IQueryable<Contest> FilterByPhase(IQueryable<Contest> result, string phase)
        {
            if (phase == string.Empty)
            {
                return result;
            }
            bool isEnumParsed = Enum.TryParse(phase, true, out PhaseEnum phaseEnum);
            return isEnumParsed ? result.Where(r => r.PhaseName == phaseEnum) : throw new InvalidInputException("Phase must be: ( one , two , finished) or (1 , 2 , 3)");
        }

        public IQueryable<Contest> FilterByType(IQueryable<Contest> result, string type)
        {
            if (type == string.Empty)
            {
                return result;
            }
            if (type.ToLower() != "true" && type.ToLower() != "false" && type.ToLower() != "open" && type.ToLower() != "invitation")
            {
                throw new InvalidInputException("Type must be: (True,  False)  or (Open , Invitation) ");
            }
            return (type.ToLower() == "true"|| type.ToLower() == "open") ? result.Where(r => r.IsOpen == true) : result.Where(r => r.IsOpen == false);
        }

        private static IQueryable<Contest> SortBy(IQueryable<Contest> result, string sortcriteria)
        {
            switch (sortcriteria)
            {
                case "title":
                    return result.OrderBy(result => result.Title);
                case "category":
                    return result.OrderBy(result => result.Category.Name);
                case "phase":
                    return result.OrderBy(result => result.PhaseName);
                case "type":
                    return result.OrderBy(result => result.IsOpen);
                default:
                    return result;
            }
        }
        private static IQueryable<Contest> Order(IQueryable<Contest> result, string sortorder)
        {
            return (sortorder == "desc") ? result.Reverse() : result;
        }

        // ContestUser methods
        public async Task<List<ContestUser>> GetContestUserByContestId(int contestId)
        {
            var contestUser = await this.ContestUserQuery.Where(contestUser => contestUser.ContestId == contestId).ToListAsync();
            return contestUser ?? throw new EntityNotFoundException($"Contest with {contestId} not found in ContestUser! ");
        }
        public async Task<bool> ContestExists(string title)
        {
            var IsExistsContest = await this.context.Contests.AnyAsync(c => c.Title.ToLower() == title.ToLower() && c.IsDeleted != true); 
            return IsExistsContest;
        }

        public async Task<bool> ContestUserExists(int contestId, int userId)
        {
            var IsExistsContestUser = await this.ContestUserQuery.AnyAsync(cu => cu.ContestId == contestId && cu.UserId == userId);
            return IsExistsContestUser;
        }
    }
}
