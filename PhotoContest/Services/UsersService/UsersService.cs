using PhotoContest.Exceptions;
using PhotoContest.Models;
using PhotoContest.Models.Dtos.User;
using PhotoContest.Models.QueryParameters;
using PhotoContest.Repositories.UsersRepository;
using PhotoContest.Services.UsersService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoContest.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private const string MODIFY_USER_ERROR_MESSAGE = "You can not update or delete a user!";
        private const string VIEW_USER_ERROR_MESSAGE = "You can not view username information!";


        private readonly IUsersRepository usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<User> CreateUser(User user, string password)
        {
            if (user.RoleId != 1)
            {
                throw new UnauthorizedException(MODIFY_USER_ERROR_MESSAGE);
            }
            return await this.usersRepository.CreateUser(user, password);
        }

        public async Task<User> DeleteUser(int id, User user)
        {
            if(user.RoleId != 1)
            {
                throw new UnauthorizedException(MODIFY_USER_ERROR_MESSAGE);
            }

            try
            {
                return await this.usersRepository.DeleteUser(id);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.usersRepository.GetAllUsers();
        }

        public async Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters)
        {
            return await this.usersRepository.GetAllUsers(filterParameters);
        }

        public async Task<IEnumerable<User>> GetAllUsers(UserQueryParameter filterParameters, User userLog)
        {
            if(userLog.RoleId != 1)
            {
                throw new UnauthorizedException(VIEW_USER_ERROR_MESSAGE);
            }

            return await this.usersRepository.GetAllUsers(filterParameters);
        }

        public async Task<User> GetUserById(int id)
        {    
            try
            {
                return await this.usersRepository.GetUserById(id);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }

        public async Task<User> GetUserById(int id, User logUser)
        {
            if (logUser.RoleId != 1)
            {
                throw new UnauthorizedException(VIEW_USER_ERROR_MESSAGE);
            }

            try
            {
                return await this.usersRepository.GetUserById(id);
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException("User not found!");
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                return await this.usersRepository.GetUserByUsername(username);
            }
            catch (EntityNotFoundException e)
            {
                throw new EntityNotFoundException(e.Message);
            }
        }


        public async Task<User> UpdateUser(int id, UpdateUserDto updateUser, User userLogin, string password)
        {
            if (userLogin.RoleId != 1)
            {
                throw new UnauthorizedException(MODIFY_USER_ERROR_MESSAGE);
            }

            bool dublicateExists = true;

            try
            {
                var existingUser = await this.usersRepository.GetUserByUsername(updateUser.Username);
                if (existingUser.Username == updateUser.Username)
                {
                    dublicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                dublicateExists = false;
            }

            if (dublicateExists)
            {
                throw new DuplicateEntityException("Username already exitst!");
            }

            var newUser = await this.usersRepository.UpdateUser(id, updateUser, password);
            return newUser;
        }
    }
}
