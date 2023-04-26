using PhotoContest.Dtos.Contests;
using PhotoContest.Models.Enums;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Models.ViewModels;
using PhotoContest.Services.ContestsService;
using PhotoContest.Services.UsersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models.Mappers
{
    public class ContestMapper : IContestMapper
    {
        public ContestResponseDto ConvertContestToContestResponseDto(Contest contest)
        {
            var contestResponseDto = new ContestResponseDto(contest);

            return contestResponseDto;
        }

        public List<ContestResponseDto> ConvertContestsToContestsResponseDto(List<Contest> contests)
        {
            List<ContestResponseDto> model = contests.Select(contest => new ContestResponseDto(contest)).ToList();

            return model;
        }

        public Contest ConvertContestCreateDtoToContest(ContestCreateDto contestCreateDto, Category category)
        {
            var contest = new Contest()
            {
                Title = contestCreateDto.Title,
                CategoryId = category.Id,
                Phase1Start = contestCreateDto.Phase1Start,
                Phase2Start = contestCreateDto.Phase2Start,
                EndDate = contestCreateDto.EndDate,
                PhaseName=PhaseEnum.One,
                IsOpen = contestCreateDto.IsOpen,                
            };

            return contest;
        }

        public Contest ConvertViewToModel(ContestViewModel viewModel)
        {
            var contest = new Contest()
            {
                Title = viewModel.Title,
                Phase1Start = viewModel.Phase1Start,
                Phase2Start = viewModel.Phase2Start,
                EndDate = viewModel.EndDate,
                PhaseName = PhaseEnum.One,
                IsOpen = viewModel.IsOpen,

                CategoryId = viewModel.CategoryId
            };
            return contest;
        }

        public Contest ConvertViewToModelForUpdate(ContestViewModel viewModel, Contest oldContest)
        {
            Contest model = oldContest;

            model.Title = viewModel.Title;
            model.Phase1Start = viewModel.Phase1Start;
            model.Phase2Start = viewModel.Phase2Start;
            model.EndDate = viewModel.EndDate;
            model.PhaseName = viewModel.PhaseName;
            model.IsOpen = viewModel.IsOpen;
            model.CategoryId = viewModel.CategoryId;

            //
            var userContestToAdd = new ContestUser()
            {
                UserId = viewModel.UserId,
                ContestId = oldContest.Id,
                IsJury = viewModel.IsJury

            };

            model.Users.Add(userContestToAdd);

            return model;
        }

        public ContestViewModel ConvertModelToView(Contest contest)
        {
            var viewModel = new ContestViewModel()
            {
                Title = contest.Title,
                Phase1Start = contest.Phase1Start,
                Phase2Start = contest.Phase2Start,
                EndDate = contest.EndDate,
                PhaseName = contest.PhaseName,  //
                IsOpen = contest.IsOpen,

                CategoryId = contest.CategoryId
            };
            return viewModel;
        }

        public Contest ConvertContestUpdateDtoToContest(ContestUpdateDto dto, Contest oldContest, Category categoryFromDto)
        {
            var model = oldContest;

            model.Title = dto.Title;
            model.CategoryId = categoryFromDto.Id;  
            model.Phase1Start = dto.Phase1Start;
            model.Phase2Start = dto.Phase2Start;
            model.EndDate = dto.EndDate;
            model.PhaseName = dto.PhaseName;
            model.IsOpen = dto.IsOpen;

            return model;
        }
        
        public ContestDetailViewModel ConvertToViewModel(Contest contest)
        {
            var viewModel = new ContestDetailViewModel();

            viewModel.Id = contest.Id;
            viewModel.Title = contest.Title;
            viewModel.Category = contest.Category;
            viewModel.CategoryId = contest.CategoryId;
            viewModel.Phase1Start = contest.Phase1Start;
            viewModel.Phase2Start = contest.Phase2Start;
            viewModel.EndDate = contest.EndDate;
            viewModel.PhaseName = contest.PhaseName;
            viewModel.IsOpen = contest.IsOpen;
            viewModel.Users = contest.Users;
            viewModel.PhotoPosts = contest.PhotoPosts;

            return viewModel;
        }
    }
}
