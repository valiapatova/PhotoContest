using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using PhotoContest.Data;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.RatingsRepository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.PhotoPostsRepository
{
    public class PhotoPostsRepository : IPhotoPostsRepository
    {
        private readonly DataContext dataContext;
        private readonly IRatingsRepository ratingsRepository;
        private readonly IConfiguration configuration;

        public PhotoPostsRepository(DataContext dataContext, IRatingsRepository ratingsRepository, IConfiguration configuration )
        {
            this.dataContext = dataContext;
            this.ratingsRepository = ratingsRepository;
            this.configuration = configuration;
        }

        private IQueryable<PhotoPost> PhotoPostQuery
        {
            get
            {
                return this.dataContext.PhotoPosts.Where(photoPost => photoPost.IsDeleted == false)
                    .Include(photoPost => photoPost.Ratings);
            }
        }

        public int PhotoPostsCount
        {
            get
            {
                return this.dataContext.PhotoPosts.ToList().Count;
            }
        }

        public async Task<PhotoPost> GetPhotoPostById(int id)
        {
            return await this.PhotoPostQuery.Where(photoPost => photoPost.Id == id).FirstOrDefaultAsync()
                ?? throw new EntityNotFoundException($"There is no PhotoPost with ID {id}!");
        }

        public async Task<List<PhotoPost>> GetAllPhotoPosts()
        {
            return await this.PhotoPostQuery.ToListAsync();
        }

        public async Task<List<PhotoPost>> GetByAuthorUsername(string username)
        {
            return await PhotoPostQuery.Where(photoPost => photoPost.User.Username.Contains(username)).ToListAsync()
                ?? throw new EntityNotFoundException($"User with username {username} not found!");
        }

        public async Task<List<PhotoPost>> GetByAuthorUserId(int id)
        {
            return await PhotoPostQuery.Where(photoPost => photoPost.UserId == id).ToListAsync()
                ?? throw new EntityNotFoundException($"User with ID {id} not found!");
        }

        public async Task<List<PhotoPost>> GetByContestTitle(string contestTitle)
        {
            return await PhotoPostQuery.Where(photoPost => photoPost.Contest.Title.Contains(contestTitle)).ToListAsync()
                ?? throw new EntityNotFoundException($"Contest with title {contestTitle} not found!");
        }

        public async Task<List<PhotoPost>> GetByContestId(int id)
        {
            return await PhotoPostQuery.Where(photoPost => photoPost.ContestId == id).ToListAsync()
                ?? throw new EntityNotFoundException($"Contest with ID {id} not found!");
        }
        public async Task<List<PhotoPost>> GetAllPhotoPosts(PhotoPostQueryParameters filterParameters)
        {
            string title = !string.IsNullOrEmpty(filterParameters.Title) ? filterParameters.Title.ToLowerInvariant() : string.Empty;
            string username = !string.IsNullOrEmpty(filterParameters.Username) ? filterParameters.Username.ToLowerInvariant() : string.Empty;
            string contestTitle = !string.IsNullOrEmpty(filterParameters.ContestTitle) ? filterParameters.ContestTitle.ToLowerInvariant() : string.Empty;
            string rating = !string.IsNullOrEmpty(filterParameters.Rating) ? filterParameters.Rating.ToLowerInvariant() : string.Empty;

            string sortCriteria = !string.IsNullOrEmpty(filterParameters.SortBy) ? filterParameters.SortBy.ToLowerInvariant() : string.Empty;
            string sortOrder = !string.IsNullOrEmpty(filterParameters.SortOrder) ? filterParameters.SortOrder.ToLowerInvariant() : string.Empty;

            IQueryable<PhotoPost> photoPosts = this.PhotoPostQuery;

            photoPosts = FilterByTitle(photoPosts, title);
            photoPosts = FilterByUsername(photoPosts, username);
            photoPosts = FilterByContestTitle(photoPosts, contestTitle);
            photoPosts = FilterByRating(photoPosts, rating);

            photoPosts = SortBy(photoPosts, sortCriteria);
            photoPosts = Order(photoPosts, sortOrder);

            List<PhotoPost> photoPostsList = await photoPosts.ToListAsync();

            return photoPostsList;
        }

        private static IQueryable<PhotoPost> FilterByTitle(IQueryable<PhotoPost> photoPosts, string title)
        {
            return photoPosts.Where(photoPost => photoPost.Title.Contains(title));
        }

        private static IQueryable<PhotoPost> FilterByUsername(IQueryable<PhotoPost> photoPosts, string username)
        {
            return photoPosts.Where(photoPost => photoPost.User.Username.Contains(username));
        }

        private static IQueryable<PhotoPost> FilterByContestTitle(IQueryable<PhotoPost> photoPosts, string contestTitle)
        {
            return photoPosts.Where(photoPost => photoPost.Contest.Title.Contains(contestTitle));
        }

        private static IQueryable<PhotoPost> FilterByRating(IQueryable<PhotoPost> photoPosts, string rating)
        {
            int ratingNumber = int.Parse(rating);
            return photoPosts.Where(photoPost => photoPost.TotalRating > ratingNumber);
        }

        private static IQueryable<PhotoPost> SortBy(IQueryable<PhotoPost> photoPosts, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "title":
                    return photoPosts.OrderBy(photoPost => photoPost.Title);
                case "username":
                    return photoPosts.OrderBy(photoPost => photoPost.User.Username);
                case "contesttitle":
                    return photoPosts.OrderBy(photoPost => photoPost.Contest.Title);
                case "rating":
                    return photoPosts.OrderBy(photoPost => photoPost.TotalRating);
                default:
                    return photoPosts;
            }
        }

        private static IQueryable<PhotoPost> Order(IQueryable<PhotoPost> photoPosts, string sortOrder)
        {
            return (sortOrder == "desc") ? photoPosts.Reverse() : photoPosts;
        }

        public async Task<PhotoPost> CreatePhotoPost(PhotoPost photoPost, string localUrl, int authorId, int contestId)
        {
            string imageUrl = await CloudinaryUpload(localUrl);
            
            PhotoPost photoPostToCreate = photoPost;
            photoPostToCreate.ContestId = contestId;
            photoPostToCreate.UserId = authorId;
            photoPostToCreate.Url = imageUrl;

            ContestUser contestUser = new ContestUser();
            contestUser.ContestId = contestId;
            contestUser.UserId = authorId;
            contestUser.IsJury = false;

            await this.dataContext.ContestUser.AddAsync(contestUser);
            await this.dataContext.PhotoPosts.AddAsync(photoPostToCreate);
            await this.dataContext.SaveChangesAsync();

            return photoPostToCreate;
        }

        public async Task<PhotoPost> CreatePhotoPost1(PhotoPost photoPost, Stream stream, int authorId, int contestId)
        {
            string imageUrl = await CloudinaryUpload1(photoPost.Title, stream);

            PhotoPost photoPostToCreate = photoPost;
            photoPostToCreate.ContestId = contestId;
            photoPostToCreate.UserId = authorId;
            photoPostToCreate.Url= imageUrl;

            ContestUser contestUser = new ContestUser();
            contestUser.ContestId = contestId;
            contestUser.UserId = authorId;
            contestUser.IsJury = false;

            await this.dataContext.ContestUser.AddAsync(contestUser);
            await this.dataContext.PhotoPosts.AddAsync(photoPostToCreate);
            await this.dataContext.SaveChangesAsync();

            return photoPostToCreate;
        }

        public async Task<string> CloudinaryUpload1(string title, Stream stream)
        {
            var cloudName = configuration.GetValue<string>("AccountSettings:CloudName");
            var apiKey = configuration.GetValue<string>("AccountSettings:ApiKey");
            var apiSecret = configuration.GetValue<string>("AccountSettings:ApiSecret");

            var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudName };
            Cloudinary cloudinary = new Cloudinary(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(title, stream)
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            string uploeadedUrl = uploadResult.SecureUrl.ToString();

            return uploeadedUrl;
        }

        public async Task<string> CloudinaryUpload(string imageAsString)
        {
            var cloudName = configuration.GetValue<string>("AccountSettings:CloudName");
            var apiKey = configuration.GetValue<string>("AccountSettings:ApiKey");
            var apiSecret = configuration.GetValue<string>("AccountSettings:ApiSecret");

            var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudName };
            Cloudinary cloudinary = new Cloudinary(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@$"{imageAsString}")
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            string uploeadedUrl = uploadResult.SecureUrl.ToString();

            return uploeadedUrl;
        }

        public async Task<PhotoPost> UpdatePhotoPost(int id, PhotoPost photoPost)
        {
            PhotoPost photoPostToUpdate = await GetPhotoPostById(id);

            photoPostToUpdate.Title = photoPost.Title;
            photoPostToUpdate.Story = photoPost.Story;
            photoPostToUpdate.Url = photoPost.Url;
            photoPostToUpdate.ImageName = photoPost.ImageName;

            await this.dataContext.SaveChangesAsync();

            return photoPostToUpdate;
        }

        public async Task<PhotoPost> DeletePhotoPost(int id)
        {
            var photoPostToDelete = await GetPhotoPostById(id);
            photoPostToDelete.IsDeleted = true;

            List<Rating> ratings = await ratingsRepository.GetByPhotoPostId(id);
            foreach (Rating rating in ratings)
            {
                rating.IsDeleted = true;
            }

            await this.dataContext.SaveChangesAsync();

            return photoPostToDelete;
        }
    }
}
