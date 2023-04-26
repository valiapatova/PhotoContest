using PhotoContest.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models
{
    public class Contest
    {
        [Key]
        public int Id { get; set; }
        [Required]     
        public string Title { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime Phase1Start { get; set; }
        public DateTime Phase2Start { get; set; }
        public DateTime EndDate { get; set; }
        public PhaseEnum PhaseName { get; set; } 
        public bool IsDeleted { get; set; }
        public ICollection<ContestUser> Users { get; set; } = new List<ContestUser>();
        public ICollection<PhotoPost> PhotoPosts { get; set; }
    }
}
