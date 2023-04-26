namespace PhotoContest.Dtos.Rating
{
    public class CreateRatingDto
    {
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public int PhotoPostId { get; set; }
        public int UserId { get; set; }
    }
}
