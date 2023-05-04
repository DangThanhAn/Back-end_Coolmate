using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using System.Data;

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
        [HttpPut("UpdateTotalPrice")]
        public IActionResult UpdateTotalPrice(int id,decimal totalPrice)
        {
            var cart = _context.Carts.Where(c => c.Id ==id).FirstOrDefault();
            if (cart != null)
            {
                cart.TotalPrice = totalPrice;
                _context.SaveChanges();
                return Ok(new { message = "Updated Total Price" });
            }
            return BadRequest(new { message = "Cart detail not found" });
        }


        [HttpPost("Order")]
        public IActionResult CreateOrder(Order order)
        {
            if (order != null)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return Ok(new { order });
            }
            return BadRequest(new { message = "Order detail not found" });
        }
        [HttpPost("OrderDetails")]
        public IActionResult CreateOrderDetail(OrdersDetail orderDetails)
        {
            if (orderDetails != null)
            {
                _context.OrdersDetails.Add(orderDetails);
                _context.SaveChanges();
                return Ok(new { orderDetails });
            }
            return BadRequest(new { message = "OrdersDetails detail not found" });
        }

        [HttpGet("GetNumberInCart")]
        public int GetNumberInCart(int userId)
        {
            string connectionString = "Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Trusted_Connection=True;TrustServerCertificate = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("Proc_GetNumberInCart", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    command.Parameters.Add("@KQ", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();
                    int result  = Convert.ToInt32(command.Parameters["@KQ"].Value);
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostProduct(Cart product)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'CoolmateContext.Products'  is null.");
            }
            _context.Carts.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
    }
}
