using PhotoContest.Models.Mappers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace PhotoContest.Models.Mappers
{
    public class UserMapper : IUserMapper
    {
        public IEnumerable<UserResponseGetIncludeDto> ConvertUsersToIncludeUsersDto(IEnumerable<User> users)
        {
            IEnumerable<UserResponseGetIncludeDto> usersToView = users.Select(u => new UserResponseGetIncludeDto(u));

            return usersToView;
        }
    }
}
