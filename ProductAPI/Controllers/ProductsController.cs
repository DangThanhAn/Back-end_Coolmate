using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CoolmateContext _context;

        public ProductsController(CoolmateContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var product = await _context.Products
                .Include(color => color.Colors)
                .Include(size => size.Sizes)
                .Include(img => img.Images)
                .ToListAsync();
            return product;
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products
                .Include(color => color.Colors)
                .Include(size => size.Sizes)
                .Include(img => img.Images)
                .FirstOrDefaultAsync( p=> p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'CoolmateContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPut("ReUpdateQuantity")]
        public IActionResult ReUpdateQuantity(OrdersDetail orderDetails)
        {
            string connectionString = "Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Trusted_Connection=True;TrustServerCertificate = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("Proc_UpdateQuantityAvailible", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@productId", SqlDbType.Int).Value = orderDetails.ProductId;
                    command.Parameters.Add("@quantityBuy", SqlDbType.Int).Value = orderDetails.Quantity;
                    command.Parameters.Add("@sizeText", SqlDbType.NVarChar).Value = orderDetails.Size;
                    connection.Open();

                    command.ExecuteNonQuery();

                    return NoContent();
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
        [HttpGet("GetListFeaturedProducts")]
        public IEnumerable<Product> GetListFeaturedProducts(int topK)
        {
            List<Product> bestSellingProducts = _context.Products
                    .Include(a => a.Images)
                .OrderBy(p => p.QuantityAvailable)
                .Take(topK)
                .ToList();
            return bestSellingProducts;
        }
    }
}
