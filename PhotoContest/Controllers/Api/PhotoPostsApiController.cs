using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Services.PhotoPostsServices;
using PhotoContest.Dtos.PhotoPost;
using PhotoContest.Services.UsersService;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;

namespace PhotoContest.Controllers.Api
{
    [Route("api/PhotoPosts")]
    [ApiController]
    public class PhotoPostsApiController : ControllerBase
    {
        private readonly IPhotoPostsService photoPostService;
        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IPhotoPostMapper photoPostMapper;

        public PhotoPostsApiController(IPhotoPostsService photoPostService, IAuthorizationHelper authorizationHelper, IPhotoPostMapper photoPostMapper)
        {
            this.photoPostService = photoPostService;
            this.authorizationHelper = authorizationHelper;
            this.photoPostMapper = photoPostMapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPhotoPostById([FromHeader] string userLogName, [FromHeader] string password,
            [FromRoute] int id)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                PhotoPost photoPost = await this.photoPostService.GetPhotoPostById(id);

                return Ok(photoPost);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPhotoPosts([FromHeader] string userLogName, [FromHeader] string password, 
            [FromQuery] PhotoPostQueryParameters filterParameters)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                List<PhotoPost> photoPosts = await this.photoPostService.GetAllPhotoPosts(filterParameters);

                return Ok(photoPosts);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("Contests/{contestId}/Create")]
        public async Task<ActionResult> Create([FromHeader] string userLogName, [FromHeader] string password, 
            [FromBody] CreatePhotoPostDto dto, int contestId)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);
                int authorId = userLog.Id;

                string localUrl = dto.LocalUrl;
                PhotoPost photoPost = this.photoPostMapper.Convert(dto);
                PhotoPost newPhotoPost = await this.photoPostService.CreatePhotoPost(photoPost, localUrl, authorId, contestId);

                return Ok(newPhotoPost);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromHeader] string userLogName, [FromHeader] string password,
            [FromRoute] int id, [FromBody] UpdatePhotoPostDto dto)
        {            
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                PhotoPost photoPost = this.photoPostMapper.Convert(dto);
                PhotoPost photoPostToUpdate = await this.photoPostService.UpdatePhotoPost(id, photoPost, userLog);

                return Ok(photoPostToUpdate);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedException e)
            {
                return NotFound(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromHeader] string userLogName, [FromHeader] string password, 
            [FromRoute] int id)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                PhotoPost photoPost = new PhotoPost();
                PhotoPost photoPostToDelete = await this.photoPostService.DeletePhotoPost(id, userLog);

                return Ok(photoPostToDelete);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedException e)
            {
                return NotFound(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
