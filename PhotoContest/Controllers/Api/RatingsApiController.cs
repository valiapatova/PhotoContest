using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.Rating;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Services.RatingsService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Controllers.Api
{
    [Route("api/Ratings")]
    [ApiController]
    public class RatingsApiController : ControllerBase
    {
        private readonly IRatingsService ratingService;
        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IRatingMapper ratingMapper;

        public RatingsApiController(IRatingsService ratingService, IAuthorizationHelper authorizationHelper, IRatingMapper ratingMapper)
        {
            this.ratingService = ratingService;
            this.authorizationHelper = authorizationHelper;
            this.ratingMapper = ratingMapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRatingById([FromHeader] string userLogName, [FromHeader] string password, 
            [FromRoute] int id)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                Rating rating = await this.ratingService.GetRatingById(id);

                return Ok(rating);
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
        public async Task<ActionResult> GetAllRatings([FromHeader] string userLogName, [FromHeader] string password, 
            [FromQuery] RatingQueryParameters filterParameters)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                List<Rating> photoPosts = await this.ratingService.GetAllRatings(filterParameters);

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

        [HttpPost("PhotoPosts/{photoPostId}/Create")]
        public async Task<ActionResult> Create([FromHeader] string userLogName, [FromHeader] string password,
            [FromBody] CreateRatingDto dto, int photoPostId)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                Rating rating = this.ratingMapper.Convert(dto);
                Rating newRating = await this.ratingService.CreateRating(rating, userLog, photoPostId);

                return Ok(newRating);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromHeader] string userLogName, [FromHeader] string password, 
            [FromRoute] int id, [FromBody] UpdateRatingDto dto)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                Rating rating = this.ratingMapper.Convert(dto);
                Rating ratingToUpdate = await this.ratingService.UpdateRating(id, rating, userLog);

                return Ok(ratingToUpdate);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromHeader] string userLogName, [FromHeader] string password,
            [FromRoute] int id)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                Rating rating = new Rating();
                Rating ratingtToDelete = await this.ratingService.DeleteRating(id, userLog);

                return Ok(ratingtToDelete);
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
    }
}
