using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("UpdateQuantity")]
        public IActionResult UpdateQuantity(int cartDetailId, int quantity)
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

        [HttpGet("UpdateSize")]
        public IActionResult UpdateSizePr(int cartDetailId, string size)
        {
            var cartDetails = _context.CartDetails.FirstOrDefault(p => p.CartDetailId == cartDetailId);
            if (cartDetails != null)
            {
                // Cập nhật thuộc tính mới cho bản ghi
                cartDetails.Size = size;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return Ok(new { message = "Updated size success" });
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
        [HttpDelete("ClearCart")]
        public IActionResult ClearCart(int cartId)
        {
            string connectionString = "Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Trusted_Connection=True;TrustServerCertificate = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM CartDetail WHERE CartId = @CartId", connection))
                {
                    command.Parameters.AddWithValue("@CartId", cartId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return Ok(new { message = $"{rowsAffected} Row Deleted" });
                }
            }
        }



        [HttpPost("CreateCartDetail")]
        public async Task<IActionResult> CreateCartDetail(CartDetail c)
        { 
            if (_context.Products == null)
            {
                return Problem("Entity set 'CoolmateContext.Products'  is null.");
            }
            _context.CartDetails.Add(c);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Created" });
        }

        [HttpGet("GetListCart")]
        public async Task<ActionResult<IEnumerable<CartDetail>>> GetCartDetails(int cartId)
        {
            if (_context.CartDetails == null)
            {
                return NotFound();
            }
            return await _context.CartDetails.Where(c => c.CartId == cartId).Include(p => p.Product).ToListAsync();
        }
    }
}
