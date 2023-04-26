using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PhotoContest.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Contest> Constests { get; set; }
    }
}
