using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public CartController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCart(int userId) // Lấy ra giỏ hàng của khách hàng this và trả về một đối tượng gồm thông tin cart, cartdetail và các sản phẩm trong details đó
        {
            var cart = _context.Carts
                .Include(cartdetail => cartdetail.CartDetails)
                 .ThenInclude(cd => cd.Product)
                        .ThenInclude(p => p.Images)
                .Where(c => c.UserId == userId);

            //var detail = _context.CartDetails.Include(p => p.Product).Where(c => c.ProductId == p.Id);
            //details sẽ gộp với product
            //var product = _context.Products
            //   .Include(color => color.Colors)
            //   .Include(size => size.Sizes)
            //   .Include(img => img.Images)
            //   .FirstOrDefaultAsync(p => p.Id == 1);

            //var data = from cd in _context.CartDetails
            //           join p in _context.Products
            return Ok(cart);
        }
      
    }
}
