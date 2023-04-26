using System.Collections.Generic;

namespace PhotoContest.Models.Mappers.Contracts
{
    public interface IUserMapper
    {
        IEnumerable<UserResponseGetIncludeDto> ConvertUsersToIncludeUsersDto(IEnumerable<User> users);
    }
}
