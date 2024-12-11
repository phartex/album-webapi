using albumwebapi.Interfacers;
using albumwebapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace albumwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            var createdReview = await _reviewService.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReviews), createdReview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (result == 0)
            {
                // If no rows were affected, return NotFound
                var notFoundResponse = new DeleteMethodResponse(404, "Review not found");
                return NotFound(notFoundResponse);
            }

            var successResponse = new DeleteMethodResponse(204, "Review deleted successfully");

            // Return a status of 204 (No Content) to indicate successful deletion
            return Ok(successResponse);
        }
    }
}