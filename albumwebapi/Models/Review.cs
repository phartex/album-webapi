namespace albumwebapi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CreateReviewDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class EncryptedReview
    {
        public string Name { get; set; } // Encrypted 'Name' from frontend
        public string Content { get; set; } // Encrypted 'Content' from frontend
    }

}
