using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Services.PhotoPostsServices;
using PhotoContest.Services.RatingsService;
using PhotoContest.Services.UsersService;
using System.IO;
using System.Threading.Tasks;

namespace PhotoContest.Controllers
{
    public class PhotoPostController : Controller
    {
        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IUsersService usersService;
        private readonly IPhotoPostsService photoPostsService;
        private readonly IConfiguration configuration;
        private readonly IRatingsService ratingsService;

        public PhotoPostController(IAuthorizationHelper authorizationHelper, IUsersService usersService,
            IPhotoPostsService photoPostsService, IConfiguration configuration, IRatingsService ratingsService)
        {
            this.authorizationHelper = authorizationHelper;
            this.usersService = usersService;
            this.photoPostsService = photoPostsService;
            this.configuration = configuration;
            this.ratingsService = ratingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int contestId)
        {
            try
            {
                string currentUser = this.HttpContext.Session.GetString("CurrentUser");
                User user = await usersService.GetUserByUsername(currentUser);

                PhotoPost photoPost = new PhotoPost();
                photoPost.ContestId = contestId;
                photoPost.UserId = user.Id;

                return this.View(model: photoPost);
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
            catch (AuthorizationException e)
            {
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View(viewName: "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PhotoPost photoPost, int userId, int contestId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Stream stream = photoPost.ImageFile.OpenReadStream();
                    string title = photoPost.Title;

                    var newPhotoPost = await this.photoPostsService.CreatePhotoPost1(photoPost, stream, userId, contestId);

                    return this.RedirectToAction(actionName: "CreatedPhoto", controllerName: "PhotoPost");
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
                catch (AuthorizationException e)
                {
                    this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    this.ViewData["ErrorMessage"] = e.Message;

                    return this.View(viewName: "Error");
                }
            }

            return this.RedirectToAction(actionName: "Index", controllerName: "Contests");
        }

        [HttpGet]
        public async Task<IActionResult> Rate(int photoPostId)
        {
            try
            {
                string currentUser = this.HttpContext.Session.GetString("CurrentUser");
                User user = await usersService.GetUserByUsername(currentUser);

                Rating rating = new Rating();
                rating.UserId = user.Id;
                rating.PhotoPostId = photoPostId;

                return this.View(model: rating);
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
            catch (AuthorizationException e)
            {
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View(viewName: "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Rate([FromForm] Rating rating, int userId, int photoPostId)
        {
            try
            {
                User userLog = await this.usersService.GetUserById(userId);

                var ratingToCreate = await this.ratingsService.CreateRating(rating, userLog, photoPostId);

                return this.RedirectToAction(actionName: "Index", controllerName: "Contests");
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
            catch (AuthorizationException e)
            {
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View(viewName: "Error");
            }
        }

        public IActionResult CreatedPhoto()
        {
            return this.View();
        }
    }
}

