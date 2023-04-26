using PhotoContest.Models;
using PhotoContest.Models.QueryParameters;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PhotoContest.Services.PhotoPostsServices
{
    public interface IPhotoPostsService
    {
        Task<PhotoPost> GetPhotoPostById(int id);
        Task<List<PhotoPost>> GetAllPhotoPosts(PhotoPostQueryParameters filterParameters);
        Task<PhotoPost> CreatePhotoPost(PhotoPost photoPost, string localUrl, int authorId, int contestId);
        Task<PhotoPost> CreatePhotoPost1(PhotoPost photoPost, Stream stream, int authorId, int contestId);
        Task<PhotoPost> UpdatePhotoPost(int id, PhotoPost photoPost, User userLog);
        Task<PhotoPost> DeletePhotoPost(int id, User userLog);
    }
}
