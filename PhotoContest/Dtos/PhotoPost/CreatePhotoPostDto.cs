using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContest.Dtos.PhotoPost
{
    public class CreatePhotoPostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Story { get; set; }
        
        public string LocalUrl { get; set; }
    }
}
