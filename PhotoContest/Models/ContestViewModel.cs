using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoContest.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class ContestViewModel
    {
        [Required]
        public string Title { get; set; }
        
        [Display(Name = "Category")]
        [Required(ErrorMessage = "The {0} field is required")]
        public int CategoryId { get; set; }

        [Required]
        public DateTime Phase1Start { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Phase 1 (Upload) Deadline:")]
        public DateTime Phase2Start { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Phase 2 (Rate) Deadline")]
        public DateTime EndDate { get; set; } = DateTime.Now;
        [Required]
        public PhaseEnum PhaseName { get; set; }= PhaseEnum.One;
        
        [Required]
        [Display(Name = "Type: ")]
        public bool IsOpen { get; set; } = true;
        
        public SelectList Categories { get; set; }

        //
        public bool IsJury { get; set; }
        [Display(Name = "Username")]
        public int UserId { get; set; }
        //public int ContestId { get; set; }
        public SelectList Users { get; set; }

    }
}

