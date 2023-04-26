using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Services.UsersService;
using System.Threading.Tasks;

namespace PhotoContest.Controllers
{
    public class AdminsController : Controller
    {
        private readonly IUsersService userService;

        public AdminsController(IUsersService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userService.GetAllUsers();

            return View(model: users);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                User user = await this.userService.GetUserById(id);

                return this.View(model: user);
            }
            catch (EntityNotFoundException)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = $"User with id {id} does not exist.";

                return this.View(viewName: "Error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                User user = await this.userService.GetUserById(id);
                return this.View(user);
            }
            catch (EntityNotFoundException)
            {
                return this.UserNotFound(id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                string currentUser = this.HttpContext.Session.GetString("CurrentUser");

                User user = await this.userService.GetUserByUsername(currentUser);

                await this.userService.DeleteUser(id, user);

                return this.RedirectToAction(actionName: "Index", controllerName: "Admins");
            }
            catch(UnauthorizedException e)
            {
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = e.Message;
                return this.View(viewName: "Error");
            }
            catch(EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        private IActionResult UserNotFound(int id)
        {
            this.Response.StatusCode = StatusCodes.Status404NotFound;
            this.ViewData["ErrorMessage"] = $"User with id {id} does not exist.";
            return this.View(viewName: "Error");
        }
    }
}
