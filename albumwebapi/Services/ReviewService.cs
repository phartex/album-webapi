using albumwebapi.Data;
using albumwebapi.Interfacers;
using albumwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace albumwebapi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

      

        public async Task<Review> AddReviewAsync(Review review)
        {

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<int> DeleteReviewAsync(int id)
        {
           var review = await _context.Reviews.FirstOrDefaultAsync(review => review.Id == id);

            if(review == null)
            {
                return 0;
            }
            _context.Reviews.Remove(review);

            return await _context.SaveChangesAsync();

        }

        public async Task<List<Review>> GetReviewsAsync()
        {
          return await _context.Reviews.ToListAsync();
        }
    }
}
