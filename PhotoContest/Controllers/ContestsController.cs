using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Models.ViewModels;
using PhotoContest.Services.CategoriesService;
using PhotoContest.Services.ContestsService;
using PhotoContest.Services.UsersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Controllers
{
    public class ContestsController : Controller
    {
		private readonly IAuthorizationHelper authorizationHelper;
		private readonly IContestsService contestsService;
		private readonly IContestMapper contestMapper;		
		private readonly ICategoriesService categoriesService;
		private readonly IUsersService usersService;

		public ContestsController(IAuthorizationHelper authorizationHelper, IContestsService contestsService, IContestMapper contestMapper,ICategoriesService categoriesService, IUsersService usersService)
		{
			this.authorizationHelper = authorizationHelper;
			this.contestsService = contestsService;
			this.contestMapper = contestMapper;
			this.categoriesService = categoriesService;
			this.usersService = usersService;
		}

		public async Task<IActionResult> Index(ContestQueryParameter filterParams)
		{
			if (!this.ModelState.IsValid)
			{
				filterParams = new ContestQueryParameter();
			}

			try
			{
				this.ViewData["SortOrder"] = string.IsNullOrEmpty(filterParams.SortOrder) ? "desc" : "";

				PaginatedList<Contest> paginatedList = await this.contestsService.GetAllPaginated(filterParams);

				return this.View(model: paginatedList);
			}
			catch (InvalidInputException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;
				return this.View(viewName: "Error");
			}
		}

		public async Task<IActionResult> Details(int id)
		{
			try
			{
				Contest contest = await this.contestsService.GetById(id);

                string currentUser = this.HttpContext.Session.GetString("CurrentUser");
                User user = await usersService.GetUserByUsername(currentUser);

                ContestDetailViewModel contestDetailViewModel = this.contestMapper.ConvertToViewModel(contest);
                contestDetailViewModel.CurrentlyLoggedUser = user;

                return this.View(model: contestDetailViewModel);
			}
			catch (EntityNotFoundException)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = $"Contest with id {id} does not exist.";

				return this.View(viewName: "Error");
			}
		}

		public async Task<IActionResult> Create()  
		{
			ContestViewModel viewModel = new ContestViewModel();

			await this.InitializeDropDownListCategory(viewModel);

			return this.View(model:viewModel);

		}

		[HttpPost]
		public async Task<IActionResult> Create(ContestViewModel viewModel)
		{
			if (!this.ModelState.IsValid)
			{
				await this.InitializeDropDownListCategory(viewModel);
				return this.View(model: viewModel);
			}
			try
			{
				string currentUser = this.HttpContext.Session.GetString("CurrentUser");
				User user = await usersService.GetUserByUsername(currentUser);	
				
				Contest contest = this.contestMapper.ConvertViewToModel(viewModel);
				await this.contestsService.Create(contest,user);
			}
            catch (EntityNotFoundException)   
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;				
				this.ViewData["ErrorMessage"] = "Please Login";

                return this.View(viewName: "Error");
            }
            catch (UnauthorizedException e)
			{
				this.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = e.Message;

				return this.View(viewName: "Error");
			}
			catch (DuplicateEntityException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;

				return this.View(viewName: "Error");
			}
			return this.RedirectToAction(actionName: "Index", controllerName: "Contests");
		}

		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				Contest contest = await this.contestsService.GetById(id);				

				ContestViewModel viewModel = this.contestMapper.ConvertModelToView(contest);

				await this.InitializeDropDownListCategory(viewModel);
				await this.InitializeDropDownListUser(viewModel); //.......

				return this.View(model: viewModel);
			}
			catch (EntityNotFoundException)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = $"Contest with id {id} does not exist.";

				return this.View(viewName: "Error");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, ContestViewModel viewModel)
		{
			if (!this.ModelState.IsValid)
			{
				await this.InitializeDropDownListCategory(viewModel);
				await this.InitializeDropDownListUser(viewModel); //.......
				return this.View(model: viewModel);
			}

			try
			{
				string currentUser = this.HttpContext.Session.GetString("CurrentUser");
				
				User user = await usersService.GetUserByUsername(currentUser);

				var oldContestToUpdate = await this.contestsService.GetById(id);

				if (! (user.Role.Name == "admin"))  //??
				{
					throw new UnauthorizedException($"You are not authorized to edit contest with Id: {id}");
				}

				bool isContestUserExists = await this.contestsService.ContestUserExists(oldContestToUpdate.Id, viewModel.UserId);   ///

				if (isContestUserExists)
                {
					var cu = oldContestToUpdate.Users.Where(uc => uc.ContestId == oldContestToUpdate.Id && uc.UserId == viewModel.UserId && uc.IsDeleted!=true).FirstOrDefault(); // && u.IsJury==dto.IsJury

					oldContestToUpdate.Users.Remove(cu);
				}

				Contest contest = this.contestMapper.ConvertViewToModelForUpdate(viewModel, oldContestToUpdate);

				await this.contestsService.Update(id, contest, user);
			}
			catch (UnauthorizedException e)
			{
				this.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = e.Message;
				return this.View(viewName: "Error");
			}
			catch (EntityNotFoundException)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = $"Contest with id {id} does not exist.";
				return this.View(viewName: "Error");
			}

			return this.RedirectToAction(actionName: "Index", controllerName: "Contests");
		}

		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				Contest contest = await this.contestsService.GetById(id);
				return this.View(contest);
			}
			catch (EntityNotFoundException)
            {
				return this.ContestNotFound(id);
            }
		}


		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(int id)		
		{
			try
			{
				string currentUser = this.HttpContext.Session.GetString("CurrentUser");

				User user = await usersService.GetUserByUsername(currentUser);
				
				await this.contestsService.Delete(id, user);

				return this.RedirectToAction(actionName: "Index", controllerName: "Contests");

			}
			catch (UnauthorizedException e)
			{
				this.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = e.Message;
				return this.View(viewName: "Error");
			}
			catch (EntityNotFoundException e)
			{
				return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
			}
		}

		private IActionResult ContestNotFound(int id)
		{
			this.Response.StatusCode = StatusCodes.Status404NotFound;
			this.ViewData["ErrorMessage"] = $"Contest with id {id} does not exist.";
			return this.View(viewName: "Error");
		}
		private async Task InitializeDropDownListCategory(ContestViewModel viewModel)
        {
			viewModel.Categories = new SelectList(await this.categoriesService.GetAll(), dataValueField: "Id", dataTextField: "Name");
        }

		private async Task InitializeDropDownListUser(ContestViewModel viewModel)
		{
			viewModel.Users = new SelectList(await this.usersService.GetAllUsers(), dataValueField: "Id", dataTextField: "Username");

			//viewModel.Users = new SelectList(await this.usersService.GetAllUsers(), dataValueField: "UserId", dataTextField: "Username");
		}


	}
}
