using PhotoContest.Dtos.Rating;

namespace PhotoContest.Models.Mappers.Contracts
{
    public interface IRatingMapper
    {
        Rating Convert(CreateRatingDto dto);
        Rating Convert(UpdateRatingDto dto);
    }
}
