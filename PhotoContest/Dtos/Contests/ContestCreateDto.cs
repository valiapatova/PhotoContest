using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Dtos.Contests
{
    public class ContestCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime Phase1Start { get; set; }
        [Required]
        public DateTime Phase2Start { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsOpen { get; set; } 
    }
}
