using PhotoContest.Dtos.Rating;
using PhotoContest.Models.Mappers.Contracts;

namespace PhotoContest.Models.Mappers
{
    public class RatingMapper : IRatingMapper
    {
        public Rating Convert(CreateRatingDto dto)
        {
            return new Rating()
            {
                RatingValue = dto.RatingValue,
                Comment = dto.Comment,
                PhotoPostId = dto.PhotoPostId,
                UserId = dto.UserId,
            };
        }

        public Rating Convert(UpdateRatingDto dto)
        {
            return new Rating()
            {
                RatingValue = dto.RatingValue,
                Comment = dto.Comment
            };
        }
    }
}
