using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashbroadController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public AdminDashbroadController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpGet("GetOrder")]
        public IActionResult GetOrder() {

            var order = _context.Orders.Count();
            return Ok(order);
        }
        [HttpGet("GetOrderInDay")]
        public IActionResult GetOrderInDay()
        {
            var today = DateTime.Today;
            var orderCount = _context.Orders.Count(o => o.OrderDate.Date == today);
            return Ok(orderCount);
        }
        [HttpGet("GetOrderInMonth")]
        public IActionResult GetOrderInMonth(int month)
        {
            var orderCount = _context.Orders.Count(o => o.OrderDate.Month == month);
            return Ok(orderCount);
        }


        [HttpGet("GetRevenue")]
        public IActionResult GetRevenue()
        {
            var totalPrice = _context.Orders.Sum(o => o.TotalPrice);
            return Ok(totalPrice);
        }
        [HttpGet("GetRevenueInMonth")]
        public IActionResult GetRevenueInMonth(int month)
        {
            var totalPrice = _context.Orders.Where(o => o.OrderDate.Month == month).Sum(o => o.TotalPrice);
            return Ok(totalPrice);
        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer()
        {
            var user = _context.Users.Count();
            return Ok(user);
        }

        [HttpGet("GetReview")]
        public IActionResult GetReview()
        {
            var user = _context.Reviews.Count();
            return Ok(user);
        }
    }
}
