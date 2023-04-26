using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.RatingsRepository
{
    public interface IRatingsRepository
    {
        Task<Rating> GetRatingById(int id);
        Task<List<Rating>> GetAllRatings();
        Task<List<Rating>> GetAllRatings(RatingQueryParameters filterParameters);
        Task<List<Rating>> GetByPhotoPostTitle(string photoPostTitle);
        Task<List<Rating>> GetByPhotoPostId(int id);
        Task<List<Rating>> GetByAuthorUsername(string username);
        Task<List<Rating>> GetByAuthorUserId(int id);
        Task<Rating> CreateRating(Rating rating);
        Task<Rating> UpdateRating(int id, Rating rating);
        Task<Rating> DeleteRating(int id);
    }
}
