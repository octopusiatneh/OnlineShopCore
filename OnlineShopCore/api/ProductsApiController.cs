using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopCore.Data.EF;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.ViewModels;

namespace OnlineShopCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductsApi
        [HttpGet]
        public IEnumerable<ProductApiViewModel> GetProducts()
        {
            List<ProductApiViewModel> productApiViewModels = new List<ProductApiViewModel>();
            var products = _context.Products;
            foreach (var item in products)
            {
                ProductApiViewModel productApi = new ProductApiViewModel(item);
                var category = _context.ProductCategories.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                productApi.Category = category.Name;
                var author = _context.Authors.Where(x => x.Id == item.AuthorId).FirstOrDefault();
                productApi.Author = author.AuthorName;
                var pulisher = _context.Publishers.Where(x => x.Id == item.PublisherId).FirstOrDefault();
                productApi.Publisher = pulisher.PublisherName;
                productApiViewModels.Add(productApi);
            }
            return productApiViewModels;
        }

        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/ProductsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        // POST: api/ProductsApi
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}