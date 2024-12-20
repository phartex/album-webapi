﻿namespace albumwebapi.Models
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

}
