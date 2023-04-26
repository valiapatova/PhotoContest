using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Repositories.PhotoPostsRepository
{
    public interface IPhotoPostsRepository
    {
        Task<PhotoPost> GetPhotoPostById(int id);
        Task<List<PhotoPost>> GetAllPhotoPosts();
        Task<List<PhotoPost>> GetAllPhotoPosts(PhotoPostQueryParameters filterParameters);
        Task<List<PhotoPost>> GetByAuthorUsername(string username);
        Task<List<PhotoPost>> GetByAuthorUserId(int id);
        Task<List<PhotoPost>> GetByContestTitle(string contestTitle);
        Task<List<PhotoPost>> GetByContestId(int id);
        Task<PhotoPost> CreatePhotoPost(PhotoPost photoPost, string localUrl, int authorId, int contestId);
        Task<PhotoPost> CreatePhotoPost1(PhotoPost photoPost, Stream stream, int authorId, int contestId);
        Task<PhotoPost> UpdatePhotoPost(int id, PhotoPost photoPost);
        Task<PhotoPost> DeletePhotoPost(int id);
    }
}
