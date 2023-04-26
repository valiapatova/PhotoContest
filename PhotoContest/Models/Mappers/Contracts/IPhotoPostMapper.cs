using PhotoContest.Dtos.PhotoPost;

namespace PhotoContest.Models.Mappers.Contracts
{
    public interface IPhotoPostMapper
    {
        public PhotoPost Convert(CreatePhotoPostDto dto);
        public PhotoPost Convert(UpdatePhotoPostDto dto);
    }
}
