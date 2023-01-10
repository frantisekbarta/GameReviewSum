namespace ReviewSum.Data
{
    public class Review
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Snippet { get; set; }
        public int? Score { get; set; }
        public int? Recommended { get; set; }
        public string? Alias { get; set; }
        public string? OutletName { get; set; }
        public string? OutletImage { get; set; }
        public List<string>? Authors { get; set; }
        public List<string>? Platforms { get; set; }
    }
}
