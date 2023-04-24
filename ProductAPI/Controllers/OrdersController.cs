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
    public class OrdersController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public OrdersController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> getOrder()
        {
            var product = await _context.Orders
                .Include( u=> u.User )
                .ToListAsync();
            return product;
        }

        [HttpGet("GetDetailsByOrderId")]
        public IActionResult GetDetailsByOrderId(int orderId)
        {
            string connectionString = "Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Trusted_Connection=True;TrustServerCertificate = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("Proc_GetOrderHistory", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Khởi tạo đối tượng DataTable
                    DataTable dataTable = new DataTable();
                    // Đổ dữ liệu từ stored procedure vào DataTable bằng phương thức Fill() của đối tượng SqlDataAdapter
                    adapter.Fill(dataTable);
                    return new JsonResult(dataTable);
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

        [HttpGet("GetAllOrderHistoryByUserId")]
        public IActionResult GetAllOrderHistoryByUserId(int userId)
        {
            string connectionString = "Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Trusted_Connection=True;TrustServerCertificate = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("Proc_GetAllOrderHistoryByUserId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Khởi tạo đối tượng DataTable
                    DataTable dataTable = new DataTable();
                    // Đổ dữ liệu từ stored procedure vào DataTable bằng phương thức Fill() của đối tượng SqlDataAdapter
                    adapter.Fill(dataTable);
                    return new JsonResult(dataTable);
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
    }
}
