using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailsController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public CartDetailsController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpGet]
        public IActionResult UpdateSize(int cartDetailId, int quantity)
        {
            var cartDetails = _context.CartDetails.FirstOrDefault(p => p.CartDetailId == cartDetailId);
            if (cartDetails != null)
            {
                // Cập nhật thuộc tính mới cho bản ghi
                cartDetails.Quantity = quantity;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return Ok(new { message = "Updated" });
            }
            return BadRequest(new { message = "Cart detail not found" });
        }

        [HttpDelete]
        public IActionResult DeleteCD(int cartDetailId)
        {
            var cartDetails = _context.CartDetails.FirstOrDefault(p => p.CartDetailId == cartDetailId);
            if (cartDetails != null)
            {
                _context.CartDetails.Remove(cartDetails);
                _context.SaveChanges();
                return Ok(new { message = "Deleted" });
            }
            return BadRequest(new { message = "Cart detail not found" });
        }
    }
}
