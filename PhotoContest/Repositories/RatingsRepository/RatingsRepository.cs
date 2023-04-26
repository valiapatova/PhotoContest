using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.RatingsRepository
{
    public class RatingsRepository : IRatingsRepository
    {
        private readonly DataContext dataContext;
        public RatingsRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        private IQueryable<Rating> RatingQuery
        {
            get
            {
                return this.dataContext.Ratings.Where(rating => rating.IsDeleted == false)
                    .Include(rating => rating.User)
                    .Include(rating => rating.PhotoPost);
            }
        }

        public int RatingCount
        {
            get
            {
                return this.dataContext.Ratings.ToList().Count;
            }
        }

        public async Task<Rating> GetRatingById(int id)
        {
            return await this.RatingQuery.Where(rating => rating.Id == id).FirstOrDefaultAsync()
                ?? throw new EntityNotFoundException($"There is no Rating with ID {id}!");
        }

        public async Task<List<Rating>> GetByPhotoPostTitle(string photoPostTitle)
        {
            return await RatingQuery.Where(rating => rating.PhotoPost.Title.Contains(photoPostTitle)).ToListAsync()
                ?? throw new EntityNotFoundException($"Photo Post with title {photoPostTitle} not found!");
        }

        public async Task<List<Rating>> GetByPhotoPostId(int id)
        {
            return await RatingQuery.Where(rating => rating.PhotoPost.Id == id).ToListAsync()
                ?? throw new EntityNotFoundException($"Photo Post with ID {id} not found!");
        }

        public async Task<List<Rating>> GetByAuthorUsername(string username)
        {
            return await RatingQuery.Where(rating => rating.User.Username.Contains(username)).ToListAsync()
                ?? throw new EntityNotFoundException($"User with username {username} not found!");
        }

        public async Task<List<Rating>> GetByAuthorUserId(int id)
        {
            return await RatingQuery.Where(rating => rating.User.Id == id).ToListAsync()
                ?? throw new EntityNotFoundException($"User with ID {id} not found!");
        }

        public async Task<List<Rating>> GetAllRatings()
        {
            return await this.RatingQuery.ToListAsync();
        }
        public async Task<List<Rating>> GetAllRatings(RatingQueryParameters filterParameters)
        {
            string rating = !string.IsNullOrEmpty(filterParameters.RatingValue) ? filterParameters.RatingValue.ToLowerInvariant() : string.Empty;
            string comment = !string.IsNullOrEmpty(filterParameters.Comment) ? filterParameters.Comment.ToLowerInvariant() : string.Empty;
            string photoPostTitle = !string.IsNullOrEmpty(filterParameters.PhotoPostTitle) ? filterParameters.PhotoPostTitle.ToLowerInvariant() : string.Empty;
            string authorUsername = !string.IsNullOrEmpty(filterParameters.AuthorUsername) ? filterParameters.AuthorUsername.ToLowerInvariant() : string.Empty;

            string sortCriteria = !string.IsNullOrEmpty(filterParameters.SortBy) ? filterParameters.SortBy.ToLowerInvariant() : string.Empty;
            string sortOrder = !string.IsNullOrEmpty(filterParameters.SortOrder) ? filterParameters.SortOrder.ToLowerInvariant() : string.Empty;

            IQueryable<Rating> ratings = this.RatingQuery;

            ratings = FilterByRating(ratings, rating);
            ratings = FilterByComment(ratings, comment);
            ratings = FilterByPhotoPostTitle(ratings, photoPostTitle);
            ratings = FilterByAuthorUsername(ratings, authorUsername);

            ratings = SortBy(ratings, sortCriteria);
            ratings = Order(ratings, sortOrder);

            List<Rating> ratingsList = await ratings.ToListAsync();

            return ratingsList;
        }

        private static IQueryable<Rating> FilterByRating(IQueryable<Rating> ratings, string rating)
        {
            int ratingNumber = int.Parse(rating);
            return ratings.Where(rating => rating.RatingValue > ratingNumber);
        }

        private static IQueryable<Rating> FilterByComment(IQueryable<Rating> ratings, string comment)
        {
            return ratings.Where(rating => rating.Comment.Contains(comment));
        }

        private static IQueryable<Rating> FilterByPhotoPostTitle(IQueryable<Rating> ratings, string title)
        {
            return ratings.Where(rating => rating.PhotoPost.Title.Contains(title));
        }

        private static IQueryable<Rating> FilterByAuthorUsername(IQueryable<Rating> ratings, string username)
        {
            return ratings.Where(rating => rating.User.Username.Contains(username));
        }

        private static IQueryable<Rating> SortBy(IQueryable<Rating> photoPosts, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "rating":
                    return photoPosts.OrderBy(photoPost => photoPost.RatingValue);
                case "comment":
                    return photoPosts.OrderBy(photoPost => photoPost.Comment);
                case "photoposttitle":
                    return photoPosts.OrderBy(photoPost => photoPost.PhotoPost.Title);
                case "authorusername":
                    return photoPosts.OrderBy(photoPost => photoPost.User.Username);
                default:
                    return photoPosts;
            }
        }

        private static IQueryable<Rating> Order(IQueryable<Rating> photoPosts, string sortOrder)
        {
            return (sortOrder == "desc") ? photoPosts.Reverse() : photoPosts;
        }

        public async Task<Rating> CreateRating(Rating rating)
        {
            EntityEntry<Rating> createdRating = await this.dataContext.Ratings.AddAsync(rating);
            await this.dataContext.SaveChangesAsync();

            var ratings = await GetAllRatings();
            var ratingsForPhoto = ratings.Where(r => r.PhotoPostId == rating.PhotoPostId).ToList();

            double sum = 0.0;

            foreach (var currentRating in ratingsForPhoto)
            {
                sum += currentRating.RatingValue;
            }

            double totalRating = ratingsForPhoto.Count != 0 ? sum / ratingsForPhoto.Count : 0;
            rating.PhotoPost.TotalRating = totalRating;
            dataContext.SaveChanges();

            return createdRating.Entity;
        }

        public async Task<Rating> UpdateRating(int id, Rating rating)
        {
            Rating ratingToUpdate = await GetRatingById(id);
            
            ratingToUpdate.RatingValue = rating.RatingValue;
            ratingToUpdate.Comment = rating.Comment;

            await this.dataContext.SaveChangesAsync();

            return ratingToUpdate;
        }

        public async Task<Rating> DeleteRating(int id)
        {
            var ratingToDelete = await GetRatingById(id);
            ratingToDelete.IsDeleted = true;

            await this.dataContext.SaveChangesAsync();

            return ratingToDelete;
        }       
    }
}
