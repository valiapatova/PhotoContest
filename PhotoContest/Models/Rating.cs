using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Range(0, 10)]
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
        public PhotoPost PhotoPost { get; set; }
        public int PhotoPostId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
