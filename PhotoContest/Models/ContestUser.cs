using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models
{
    public class ContestUser
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ContestId { get; set; }
        public Contest Contest { get; set; }
        public bool IsJury { get; set; }
        public bool IsDeleted { get; set; }
    }
}
