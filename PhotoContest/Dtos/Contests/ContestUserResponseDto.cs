using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Dtos.Contests
{
    public class ContestUserResponseDto
    {
        public ContestUserResponseDto(ContestUser contestUser)
        {
           
            Username = contestUser.User.Username;
            ContestTitle = contestUser.Contest.Title;
            IsJury = contestUser.IsJury;
        }
        public string Username { get; set; }
        public string ContestTitle { get; set; }
        public bool IsJury { get; set; }
    }
}
