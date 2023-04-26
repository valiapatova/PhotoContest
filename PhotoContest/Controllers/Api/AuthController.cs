using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContest.Dtos.User;
using PhotoContest.Exceptions;
using PhotoContest.Helpers;
using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Repositories.AuthRepository;
using System.Threading.Tasks;

namespace PhotoContest.Controllers.Api
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        
        public AuthController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            try
            {
                var result = await this.authRepo.Register(
               new User
               {
                   FirstName = request.FirstName,
                   LastName = request.LastName,
                   Username = request.Username,
                   Email = request.Email
               },
               request.Password
               );

                return Ok(result);
            }
            catch(DuplicateEntityException e)
            {
                return BadRequest(e.Message);
            }           
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLoginDto request)
        {
            try
            {
                var result = await this.authRepo.Login(request.Username, request.Password);
                return Ok(result);
            }
            catch(EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(AuthenticationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
