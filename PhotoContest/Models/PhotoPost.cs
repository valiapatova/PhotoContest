using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContest.Models
{
    public class PhotoPost
    {
        [Key]
        public int Id { get; set; }
         
        [Required]
        public string Title { get; set; }
        [Required]
        public string Story { get; set; }

        [Url]
        public string Url { get; set; }

        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }
        public int UserId { get; set; }

        public Contest Contest { get; set; }
        public int ContestId { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public double TotalRating { get; set; }

        public bool HasRatingBy(User user)
        {
            foreach (var rating in Ratings)
            {
                if (rating.UserId == user.Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
