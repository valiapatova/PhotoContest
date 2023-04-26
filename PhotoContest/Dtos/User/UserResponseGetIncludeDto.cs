using System.Collections.Generic;
using System.Linq;
using PhotoContest.Models;

namespace PhotoContest
{
    public class UserResponseGetIncludeDto
    {
        public UserResponseGetIncludeDto(User user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Username = user.Username;
            this.Email = user.Email;
            this.Role = user.Role.Name;
            this.Title = user.PhotoPosts.Select(p => p.Title).ToList();
            this.ContestTitle = user.Contests.Select(c=>c.Contest.Title).ToList();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> Title { get; set; }
        public List<string> ContestTitle { get; set; }
    }
}
