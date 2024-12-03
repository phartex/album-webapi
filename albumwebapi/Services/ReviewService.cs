using albumwebapi.Data;
using albumwebapi.Interfacers;
using albumwebapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace albumwebapi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ReviewService(AppDbContext context,IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
            const string cacheKey = "reviews";

            //if (!_cache.TryGetValue(cacheKey, out List<Review> reviews))
            //{
            //    reviews = await _context.Reviews.ToListAsync();

            //    var cacheOptions = new MemoryCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            //        SlidingExpiration = TimeSpan.FromMinutes(5)
            //    };


            //    _cache.Set(cacheKey, reviews, cacheOptions);

            //}

            //return reviews;
            return await _context.Reviews.ToListAsync();
        }
    }
}
