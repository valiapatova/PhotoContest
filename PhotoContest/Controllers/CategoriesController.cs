using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.Categories;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Services.CategoriesService;
using PhotoContest.Services.UsersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IUsersService usersService;
        private readonly ICategoryMapper categoryMapper;
        public CategoriesController(ICategoriesService categoriesService, IUsersService usersService, ICategoryMapper categoryMapper)
        {
            this.categoriesService = categoriesService;
            this.usersService = usersService;
            this.categoryMapper = categoryMapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoriesService.GetAll();
            return this.View(model: categories);
        }

        public IActionResult Create()
        {
            CategoryViewModel viewModel = new CategoryViewModel();

            return this.View(model: viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model: viewModel);
            }
            try
            {
                string currentUser = this.HttpContext.Session.GetString("CurrentUser");
                User user = await  usersService.GetUserByUsername(currentUser);

                Category category = this.categoryMapper.ConvertViewToModel(viewModel);
        
                await this.categoriesService.Create(category,user);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

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

            return this.RedirectToAction(actionName: "Index", controllerName: "Categories");
        }
       
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Category category = await categoriesService.GetById(id);

                CategoryViewModel viewModel = this.categoryMapper.ConvertModelToViewForUpdate(category);

                return this.View(model: viewModel);
            }
            catch(EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return this.View(viewName: "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model: viewModel);
            }

            try
            {
                string currentUser = this.HttpContext.Session.GetString("CurrentUser");
                User user = await usersService.GetUserByUsername(currentUser);

                Category category = this.categoryMapper.ConvertViewToModel(viewModel);
                await this.categoriesService.Update(id, category,user);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
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

            return this.RedirectToAction(actionName: "Index", controllerName: "Categories");
        }
    }
}
