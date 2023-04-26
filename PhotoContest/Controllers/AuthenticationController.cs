using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Repositories.AuthRepository;
using PhotoContest.Services.UsersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthorizationHelper authHelper;        
        private readonly IAuthRepository authRepository;       

        public AuthenticationController(IAuthorizationHelper authHelper, IAuthRepository authRepository)
        {
            this.authHelper = authHelper;
            this.authRepository = authRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerViewModel = new RegisterViewModel();

            return this.View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model: registerViewModel);
            }

            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                this.ModelState.AddModelError(key: "ConfirmPassword", errorMessage: "The password and confirmation password do not match.");

                return this.View(model: registerViewModel);
            }

            try
            {
                var user = await authRepository.Register(
                    new User
                    {
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                        Email = registerViewModel.Email,
                        Username = registerViewModel.Username

                    },
                    registerViewModel.Password);
            }
            catch (DuplicateEntityException )
            {
                this.ModelState.AddModelError(key: "Username", errorMessage: "User with same username already exists.");

                return this.View(model: registerViewModel);
            }
            return this.RedirectToAction(actionName: "Login", controllerName: "Authentication");
        }
        [HttpGet]
        public IActionResult Login()
        {
            var viewModel = new LoginViewModel();

            return this.View(model: viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model: viewModel);
            }
            try
            {
                User user = await this.authHelper.TryGetUser(viewModel.Username, viewModel.Password);
                this.HttpContext.Session.SetString(key: "CurrentUser", value: user.Username);
                this.HttpContext.Session.SetString(key: "CurrentUserRole", value: user.Role.Name);

                return this.RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            catch (AuthorizationException e)            
            {
                this.ModelState.AddModelError(key: "Username", errorMessage: e.Message);

                return this.View(model: viewModel);
            }
            catch (AuthenticationException e) 
            {
                this.ModelState.AddModelError(key: "Username", errorMessage: e.Message);

                return this.View(model: viewModel);
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("CurrentUser");
            this.HttpContext.Session.Remove("CurrentUserRole");

            return this.RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
