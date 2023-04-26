using Microsoft.AspNetCore.Http;
using PhotoContest.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContest.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string PictureUrl { get; set; }

        [DisplayName("Image Name")]
        public string PictureName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ProfilePicture { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public bool IsDeleted { get; set; }

        public Rank Rank { get; set; }

        public int RankPoints { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<PhotoPost> PhotoPosts { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<ContestUser> Contests { get; set; }
    }
}
