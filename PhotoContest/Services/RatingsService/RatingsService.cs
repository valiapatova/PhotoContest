using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.PhotoPostsRepository;
using PhotoContest.Repositories.RatingsRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Services.RatingsService
{
    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository ratingsRepository;
        private readonly IPhotoPostsRepository photoPostsRepository;

        public RatingsService(IRatingsRepository ratingsRepository, IPhotoPostsRepository photoPostsRepository)
        {
            this.ratingsRepository = ratingsRepository;
            this.photoPostsRepository = photoPostsRepository;
        }

        public async Task<Rating> GetRatingById(int id)
        {
            return await ratingsRepository.GetRatingById(id);
        }

        public async Task<List<Rating>> GetAllRatings(RatingQueryParameters filterParameters)
        {
            List<Rating> ratings = await this.ratingsRepository.GetAllRatings(filterParameters);

            return ratings;
        }

        public async Task<Rating> CreateRating(Rating rating, User userLog, int photoPostId)
        {
            var photoPost = await this.photoPostsRepository.GetPhotoPostById(photoPostId);

            var contest = photoPost.Contest;
            List<ContestUser> contestUsers = contest.Users.ToList();
            var user = contestUsers.Where(contestUser => contestUser.UserId == userLog.Id).FirstOrDefault();
            if (user == null || user.IsJury == false)
            {
                throw new AuthorizationException("You are not a Jury in this contest, thus you cannot rate PhotoPosts");
            }

            List<Rating> ratings = photoPost.Ratings.ToList();
            foreach (var ratingToCheck in ratings)
            {
                if (ratingToCheck.UserId == userLog.Id)
                {
                    throw new AuthorizationException("You have already rated this PhotoPost!");
                }
            }

            Rating createdRating = await this.ratingsRepository.CreateRating(rating);

            return createdRating;
        }

        public async Task<Rating> UpdateRating(int id, Rating rating, User userLog)
        {
            var ratingToCheck = await this.ratingsRepository.GetRatingById(id);
            if (userLog.RoleId != 1 || ratingToCheck.UserId != userLog.Id)
            {
                throw new AuthorizationException("You are not authorized to update this Rating!");
            }

            Rating updatedRating = await this.ratingsRepository.UpdateRating(id, rating);

            return updatedRating;
        }

        public async Task<Rating> DeleteRating(int id, User userLog)
        {
            var ratingToCheck = await this.ratingsRepository.GetRatingById(id);
            if (userLog.RoleId != 1 || ratingToCheck.UserId != userLog.Id)
            {
                throw new AuthorizationException("You are not authorized to delete this Rating!");
            }

            Rating deletedRating = await this.ratingsRepository.DeleteRating(id);

            return deletedRating;
        }
    }
}
