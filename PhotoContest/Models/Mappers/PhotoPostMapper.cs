using PhotoContest.Dtos.PhotoPost;
using PhotoContest.Models;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Services.PhotoPostsServices;

namespace PhotoContest.Models.Mappers
{
    public class PhotoPostMapper : IPhotoPostMapper
    {
        public PhotoPost Convert(CreatePhotoPostDto dto)
        {
            return new PhotoPost()
            {
                Title = dto.Title,
                Story = dto.Story,
            };
        }

        public PhotoPost Convert(UpdatePhotoPostDto dto)
        {
            return new PhotoPost()
            {
                Title = dto.Title,
                Story = dto.Story,
                Url = dto.Url,
                ImageName = dto.ImageName,
            };
        } 
    }
}
