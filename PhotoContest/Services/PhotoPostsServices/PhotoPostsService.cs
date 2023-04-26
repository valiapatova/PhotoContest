using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.ContestsRepository;
using PhotoContest.Repositories.PhotoPostsRepository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Services.PhotoPostsServices
{
    public class PhotoPostsService : IPhotoPostsService
    {
        private readonly IPhotoPostsRepository photoPostsRepository;
        private readonly IContestsRepository contestsRepository;
        public PhotoPostsService(IPhotoPostsRepository photoPostsRepository, IContestsRepository contestsRepository)
        {
            this.photoPostsRepository = photoPostsRepository;
            this.contestsRepository = contestsRepository;
        }

        public async Task<PhotoPost> GetPhotoPostById(int id)
        {
            return await photoPostsRepository.GetPhotoPostById(id);
        }

        public async Task<List<PhotoPost>> GetAllPhotoPosts(PhotoPostQueryParameters filterParameters)
        {
            List<PhotoPost> photoPosts = await this.photoPostsRepository.GetAllPhotoPosts(filterParameters);

            return photoPosts;
        }

        public async Task<PhotoPost> CreatePhotoPost(PhotoPost photoPost, string localUrl, int authorId, int contestId)
        {
            var contest = await this.contestsRepository.GetById(contestId);
            List<ContestUser> contestUsers = contest.Users.ToList();
            foreach (var contestUser in contestUsers)
            {
                if (contestUser.UserId == authorId && contestUser.IsDeleted == false)
                {
                    throw new AuthorizationException("You already particpate in this contest!");
                }
            }

            PhotoPost createdPhotoPost = await this.photoPostsRepository.CreatePhotoPost(photoPost, localUrl, authorId, contestId);

            return createdPhotoPost;
        }

        public async Task<PhotoPost> CreatePhotoPost1(PhotoPost photoPost, Stream stream, int authorId, int contestId)
        {
            var contest = await this.contestsRepository.GetById(contestId);
            List<ContestUser> users = contest.Users.ToList();
            foreach (var user in users)
            {
                if (user.UserId == authorId)
                {
                    throw new AuthorizationException("You already particpate in this contest!");
                }
            }

            PhotoPost createdPhotoPost = await this.photoPostsRepository.CreatePhotoPost1(photoPost, stream, authorId, contestId);

            return createdPhotoPost;
        }

        public async Task<PhotoPost> UpdatePhotoPost(int id, PhotoPost photoPost, User userLog)
        {
            if (userLog.RoleId != 1)  
            {
                throw new AuthorizationException("You are not authorized to update PhotoPosts!");
            }
            PhotoPost updatedPhotoPost = await this.photoPostsRepository.UpdatePhotoPost(id, photoPost);

            return updatedPhotoPost;
        }

        public async Task<PhotoPost> DeletePhotoPost(int id, User userLog)
        {
            var photoPost = await this.photoPostsRepository.GetPhotoPostById(id);
            if (userLog.RoleId != 1 || photoPost.UserId != userLog.Id)
            {
                throw new AuthorizationException("You are not authorized to delete this PhotoPost!");
            }

            PhotoPost deletedPhotoPost = await this.photoPostsRepository.DeletePhotoPost(id);

            return deletedPhotoPost;
        }
    }
}
