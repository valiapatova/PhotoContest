using PhotoContest.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models.ViewModels
{
    public class ContestDetailViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime Phase1Start { get; set; }
        public DateTime Phase2Start { get; set; }
        public DateTime EndDate { get; set; }
        public PhaseEnum PhaseName { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ContestUser> Users { get; set; }
        public ICollection<PhotoPost> PhotoPosts { get; set; }

        public User CurrentlyLoggedUser { get; set; }
    }
}
