using PhotoContest.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PhotoContest.Models;

namespace PhotoContest.Dtos.Contests
{
    public class ContestResponseDto
    {
        public ContestResponseDto()
        {

        }
        public ContestResponseDto(Contest contest)
        {            
            Title = contest.Title;
            Id = contest.Id;
            Category = contest.Category.Name;
            Phase = contest.PhaseName;
            IsOpen = contest.IsOpen;
            Phase1Start = contest.Phase1Start;
            Phase2Start = contest.Phase2Start;
            EndDate = contest.EndDate;            
            
            if (!(contest.PhotoPosts == null))
            {
                PhotoPosts = contest.PhotoPosts.Select(u => u.Url).ToList();
            }
            if (!(contest.Users == null))
            {              
                ContestUsers = contest.Users.Select(u => new ContestUserResponseDto(u)).ToList();              
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public PhaseEnum Phase { get; set; }
        public DateTime Phase1Start { get; set; }
        public DateTime Phase2Start { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }                  
        public List<ContestUserResponseDto> ContestUsers { get; set; } = new List<ContestUserResponseDto>();        
        public ICollection<string> PhotoPosts { get; set; } = new List<string>();
    }
}
