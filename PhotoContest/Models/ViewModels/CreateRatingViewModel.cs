using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models.ViewModels
{
    public class CreateRatingViewModel
    {
        [Range(0, 10)]
        public int RatingValue { get; set; }

        public string Comment { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        public PhotoPost PhotoPost { get; set; }
        public int PhotoPostId { get; set; }
        
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
