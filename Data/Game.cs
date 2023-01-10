namespace ReviewSum.Data
{
    public class Game
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? SquareImage { get; set; }
        public string? Url { get; set; }
        public int? PercentRecommended { get; set; }
        public int? NumReviews { get; set; }
        public int? NumTopCriticReviews { get; set; }
        public int? MedianScore { get; set; }
        public int? TopCriticScore { get; set; }
        public int? TopReviewsScore { get; set; }
        public string? Description { get; set; }
        public List<string>? Companies { get; set; }
        public List<string>? Platforms { get; set; }
        public List<string>? Genres { get; set; }
    }
}
