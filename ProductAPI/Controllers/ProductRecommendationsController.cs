using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRecommendationsController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public ProductRecommendationsController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        //}
        [HttpGet]
        public IEnumerable<Product> RecommendProducts(int userId, int topK)
        {
            var userReviews = _context.Reviews.Where(r => r.UserId == userId).ToList();

            var userProducts = userReviews.Select(r => r.Product).Distinct();

            var otherReviews = _context.Reviews
                .Where(r => r.UserId != userId && userProducts.Contains(r.Product))
                .ToList();

            var userRatings = userReviews.ToDictionary(r => r.ProductId, r => r.Rating);
            var userAvgRating = userReviews.Average(r => r.Rating);

            var similarities = new Dictionary<int, double>();
            foreach (var review in otherReviews)
            {
                if (similarities.ContainsKey(review.UserId))
                {
                    similarities[review.UserId] += (double)(userRatings[review.ProductId] - review.Rating);
                }
                else
                {
                    similarities[review.UserId] = (double)(userRatings[review.ProductId] - review.Rating);
                }
            }

            var topUsers = similarities.OrderByDescending(s => s.Value).Take(topK).ToList();

            var recommendedProducts = new Dictionary<int, double>();
            foreach (var user in topUsers)
            {
                var userReview = _context.Reviews.Where(r => r.UserId == user.Key).ToList();
                foreach (var review in userReview)
                {
                    if (!userRatings.ContainsKey(review.ProductId))
                    {
                        if (recommendedProducts.ContainsKey(review.ProductId))
                        {
                            recommendedProducts[review.ProductId] += (double)(review.Rating - userAvgRating);
                        }
                        else
                        {
                            recommendedProducts[review.ProductId] = (double)(review.Rating - userAvgRating);
                        }
                    }
                }
            }

            var topProducts = recommendedProducts.OrderByDescending(p => p.Value).Take(topK).Select(p => p.Key).ToList();
            var result = _context.Products.Where(p => topProducts.Contains(p.Id)).ToList();

            if(result.Count == 0)
            {
                List<Product> bestSellingProducts = _context.Products
                    .Include(a => a.Images)
                .OrderBy(p => p.QuantityAvailable)
                .Take(topK)
                .ToList();

                if (bestSellingProducts.Count == 0)
                {
                    return null;
                }

                Random rand = new Random();
                bestSellingProducts = bestSellingProducts.OrderBy(p => rand.Next()).ToList();

                List<Product> takeProducts = bestSellingProducts.Take(2).ToList();

                return takeProducts;
            }
            return result;
        }


    }
}
