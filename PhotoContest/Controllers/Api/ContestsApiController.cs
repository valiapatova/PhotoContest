using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.Contests;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Mappers;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Services.CategoriesService;
using PhotoContest.Services.ContestsService;
using PhotoContest.Services.UsersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Controllers.Api
{
    [Route("api/Contests")]
    [ApiController]
    public class ContestsApiController : ControllerBase
    {
        private readonly IContestsService contestsService;
        private readonly ICategoriesService categoriesService;
        private readonly IUsersService usersService;
        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IContestMapper contestMapper;

        public ContestsApiController(IContestsService contestsService, 
            ICategoriesService categoriesService,
            IUsersService usersService, IContestMapper contestMapper, IAuthorizationHelper authorizationHelper)
        {
            this.contestsService = contestsService;
            this.categoriesService = categoriesService;
            this.usersService = usersService;
            this.authorizationHelper = authorizationHelper;
            this.contestMapper = contestMapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get ([FromHeader] string userLogName, [FromHeader] string password, [FromQuery] ContestQueryParameter filterParameters)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                var contests = await this.contestsService.GetAllPaginated(filterParameters);

                var contestsToView = this.contestMapper.ConvertContestsToContestsResponseDto(contests);

                return Ok(contestsToView);
            }
            catch (UnauthorizedException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidInputException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromHeader] string userLogName, [FromHeader] string password, int id)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName,password);

                var contest = await this.contestsService.GetById(id);
                var contestToView = contestMapper.ConvertContestToContestResponseDto(contest);

                return this.StatusCode(StatusCodes.Status200OK, contestToView);
            }
            catch (UnauthorizedException e) 
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }            
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromHeader] string userLogName, [FromHeader] string password, [FromBody] ContestCreateDto dto)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName,password);
                if (user.Role.Name != "admin") 
                {
                    throw new UnauthorizedException("Only organazator can create the contest");
                }
                var category = await this.categoriesService.GetByName(dto.Category);
                var contest = this.contestMapper.ConvertContestCreateDtoToContest(dto,category);              
                var createdContest = await this.contestsService.Create(contest,user);
                var contestToView = this.contestMapper.ConvertContestToContestResponseDto(createdContest);

                return this.StatusCode(StatusCodes.Status201Created, contestToView);
            }
            catch (UnauthorizedException e)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromHeader] string userLogName, [FromHeader] string password, [FromBody] ContestUpdateDto dto)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName, password);
                if (user.Role.Name != "admin") 
                {
                    throw new UnauthorizedException("Only organazator can update the contest");
                }

                var oldContestToUpdate = await this.contestsService.GetById(id);

                Category categoryFromDto = await this.categoriesService.GetByName(dto.Category);

                foreach (string userName in dto.Usernames)
                {
                    var userToAdd = await this.usersService.GetUserByUsername(userName);
                    //
                    bool isContestUserExists = await this.contestsService.ContestUserExists(oldContestToUpdate.Id, userToAdd.Id);
                    if (!isContestUserExists)
                    {                    
                      oldContestToUpdate.Users.Add(new ContestUser() { UserId = userToAdd.Id, ContestId = oldContestToUpdate.Id, IsJury = dto.IsJury });
                    }
                    else
                    {
                        var cu = oldContestToUpdate.Users.Where(u => u.ContestId == oldContestToUpdate.Id && u.UserId == userToAdd.Id ).FirstOrDefault();

                        oldContestToUpdate.Users.Remove(cu);                      

                        oldContestToUpdate.Users.Add(new ContestUser() { UserId = userToAdd.Id, ContestId = oldContestToUpdate.Id, IsJury = dto.IsJury });
                    }
                }

                var contestToUpdate = this.contestMapper.ConvertContestUpdateDtoToContest(dto, oldContestToUpdate,categoryFromDto);
               
                var updatedContest = await this.contestsService.Update(id, contestToUpdate, user);

                var contestToView = this.contestMapper.ConvertContestToContestResponseDto(updatedContest);

                return this.StatusCode(StatusCodes.Status201Created, contestToView);
            }
            catch (UnauthorizedException e)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader] string userLogName ,[FromHeader] string password)
        {
            try
            {
                var user = await this.authorizationHelper.TryGetUser(userLogName,password);
                if (user.Role.Name != "admin")  
                {
                    throw new UnauthorizedException("Only organazator can update the contest");
                }
                var deletedContest = await this.contestsService.Delete(id, user);

                var contestToView = this.contestMapper.ConvertContestToContestResponseDto(deletedContest);

                return this.StatusCode(StatusCodes.Status200OK, contestToView);
            }
            catch (UnauthorizedException e)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }            
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (AuthenticationException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
