namespace ReviewSum.Data
{
    public interface IReviewSumService
    {
        Task<List<Game>> SearchGame(string enteredName);
        Task<Game> GetGame(Game game);
        Task<List<Review>> GetReviews(string gameId);
        int GetTopReviewsScore(List<Review> reviews);
        Task<List<string>> GetSummary(List<Review> reviews);
        Task<List<string>> GetProsAndCons(List<Review> reviews);
    }
}
