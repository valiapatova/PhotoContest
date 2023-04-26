using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Services.RatingsService
{
    public interface IRatingsService
    {
        Task<Rating> GetRatingById(int id);
        Task<List<Rating>> GetAllRatings(RatingQueryParameters filterParameters);
        Task<Rating> CreateRating(Rating rating, User userLog, int photoPostId);
        Task<Rating> UpdateRating(int id, Rating rating, User userLog);
        Task<Rating> DeleteRating(int id, User userLog);
    }
}
