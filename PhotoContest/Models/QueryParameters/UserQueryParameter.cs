using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models.QueryParameters
{
    public class UserQueryParameter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
