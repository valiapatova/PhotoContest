using PhotoContest.Dtos.Contests;
using PhotoContest.Models.ViewModels;
using System.Collections.Generic;

namespace PhotoContest.Models.Mappers.Contracts
{
    public interface IContestMapper
    {
        ContestResponseDto ConvertContestToContestResponseDto(Contest contest);
        List<ContestResponseDto> ConvertContestsToContestsResponseDto(List<Contest> contests);
        Contest ConvertContestCreateDtoToContest(ContestCreateDto contestCreateDto, Category category);
        Contest ConvertViewToModel(ContestViewModel viewModel);
        Contest ConvertViewToModelForUpdate(ContestViewModel viewModel, Contest oldContest);
        ContestViewModel ConvertModelToView(Contest contest);
        Contest ConvertContestUpdateDtoToContest(ContestUpdateDto dto, Contest oldContest, Category categoryFromDto);
        ContestDetailViewModel ConvertToViewModel(Contest contest);
    }
}
