using albumwebapi.Models;

namespace albumwebapi.Interfacers
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsAsync();
        Task<Review> AddReviewAsync(Review review);
        Task<int> DeleteReviewAsync(int id);
    }
}
