using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.User;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Models.Mappers.Contracts;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Services.UsersService;
using System.Threading.Tasks;

namespace PhotoContest.Controllers.Api
{
    [Route("api/Users")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly IUsersService userService;
        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IUserMapper mapper;

        public UsersApiController(IUsersService userService, IAuthorizationHelper authorizationHelper, IUserMapper mapper)
        {
            this.userService = userService;
            this.authorizationHelper = authorizationHelper;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromHeader] string userLogName, [FromHeader] string password, [FromQuery] UserQueryParameter filterParameters)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);

                var users = await this.userService.GetAllUsers(filterParameters, userLog);

                var usersToView = this.mapper.ConvertUsersToIncludeUsersDto(users);

                return Ok(usersToView);
            }
            catch (UnauthorizedException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
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

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromHeader] string userLogName, [FromHeader] string password, [FromBody] CreateUserDto dto)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);
                var result = await this.userService.CreateUser(
              new User
              {
                  FirstName = dto.FirstName,
                  LastName = dto.LastName,
                  Username = dto.Username,
                  Email = dto.Email,
                  RoleId = 2
              },
              dto.Password); ;

                return Ok(result);
            }
            catch (DuplicateEntityException e)
            {
                return BadRequest(e.Message);
            }
            catch (UnauthorizedException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id, [FromHeader] string userLogName, [FromHeader] string password)
        {
            try
            {
                var userLog = await this.authorizationHelper.TryGetUser(userLogName, password);
                var user = await this.userService.GetUserById(id, userLog);
                return Ok(user);
            }
            catch (UnauthorizedException e)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id, [FromHeader] string userLogName, [FromHeader] string password)
        {
            try
            {
                var userlog = await this.authorizationHelper.TryGetUser(userLogName, password);
                await this.userService.DeleteUser(id, userlog);
                return Ok();
            }
            catch (UnauthorizedException e)
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromHeader] string userLogName, [FromHeader] string password, [FromBody] UserRegisterDto newUser)
        {
            try
            {
                var userlog = await this.authorizationHelper.TryGetUser(userLogName, password);

                User oldUser = await this.userService.GetUserById(id);

                var updatedUser = await this.userService.UpdateUser(id,
               new UpdateUserDto
               {
                   FirstName = newUser.FirstName,
                   LastName = newUser.LastName,
                   Username = newUser.Username,
                   Email = newUser.Email
               },
               userlog,
               newUser.Password
               );

                return Ok(updatedUser);
            }
            catch (UnauthorizedException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return BadRequest(e.Message);
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
