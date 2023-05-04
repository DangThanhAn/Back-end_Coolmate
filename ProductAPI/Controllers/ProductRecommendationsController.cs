using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // Phương thức trả về danh sách sản phẩm được gợi ý dựa trên danh sách sản phẩm đã mua của khách hàng
        //public List<Product> GetRecommendedProducts(List<Product> purchasedProducts)
        //{
        //    // Lấy ra danh sách id của các sản phẩm đã mua
        //    var purchasedProductIds = purchasedProducts.Select(p => p.Id).ToList();

        //    // Lấy ra danh sách id của tất cả các sản phẩm mà khách hàng đã mua
        //    var customerIds = _context.OrdersDetails
        //        .Where(o => o.OrderItems.Any(oi => purchasedProductIds.Contains(oi.ProductId)))
        //        .Select(o => o.CustomerId)
        //        .Distinct()
        //        .ToList();

        //    // Lấy ra danh sách sản phẩm đã mua của các khách hàng khác
        //    var otherCustomerPurchasedProducts = dbContext.OrderItems
        //        .Include(oi => oi.Order)
        //        .Where(oi => oi.Order.CustomerId != customerId && !purchasedProductIds.Contains(oi.ProductId))
        //        .Select(oi => oi.Product)
        //        .Distinct()
        //        .ToList();

        //    // Gộp danh sách sản phẩm đã mua của khách hàng và sản phẩm đã mua của các khách hàng khác
        //    var allPurchasedProducts = purchasedProducts.Union(otherCustomerPurchasedProducts);

        //    // Lấy ra danh sách sản phẩm có chứa ít nhất một từ khóa trong danh sách từ khóa của các sản phẩm đã mua
        //    var recommendedProducts = dbContext.Products
        //        .Where(p => allPurchasedProducts.Any(ap => ap.Keywords.Contains(p.Keywords)))
        //        .ToList();

        //    return recommendedProducts;
        //}
        [HttpGet]
        public List<Product> RecommendProducts(int userId, int topK)
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
                        var userAvgRating1 = topUsers.Single(u => u.Key == user.Key).Value;
                        if (recommendedProducts.ContainsKey(review.ProductId))
                        {
                            recommendedProducts[review.ProductId] += (double)(review.Rating - userAvgRating1);
                        }
                        else
                        {
                            recommendedProducts[review.ProductId] = (double)(review.Rating - userAvgRating1);
                        }
                    }
                }
            }

            var topProducts = recommendedProducts.OrderByDescending(p => p.Value).Take(topK).Select(p => p.Key).ToList();
            var result = _context.Products.Where(p => topProducts.Contains(p.Id)).ToList();

            return result;
        }

        //public IEnumerable<Product> RecommendProducts(int userId)
        //{
        //    try
        //    {
        //        // Lấy các đánh giá của người dùng
        //        var userReviews = _context.Reviews.Where(r => r.UserId == userId).ToList();

        //        // Lấy các sản phẩm đã được đánh giá bởi người dùng
        //        var userProducts = userReviews.Select(r => r.Product).Distinct();

        //        // Lấy các người dùng khác đánh giá các sản phẩm đó
        //        var otherReviews = _context.Reviews
        //           .Where(r => r.UserId != userId && userReviews.Select(ur => ur.ProductId).Contains(r.ProductId))
        //           .ToList();

        //        // Tạo một bảng tần số cho các sản phẩm
        //        var productFrequencies = new Dictionary<Product, int>();
        //        foreach (var review in otherReviews)
        //        {
        //            if (review.Product != null)
        //            {
        //                if (productFrequencies.ContainsKey(review.Product))
        //                {
        //                    productFrequencies[review.Product]++;
        //                }
        //                else
        //                {
        //                    productFrequencies[review.Product] = 1;
        //                }
        //            }

        //        }

        //        // Sắp xếp các sản phẩm theo tần số giảm dần
        //        var recommendedProducts = productFrequencies.OrderByDescending(p => p.Value)
        //            .Select(p => p.Key)
        //            .ToList();

        //        return recommendedProducts;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
