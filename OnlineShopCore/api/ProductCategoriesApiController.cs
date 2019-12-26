using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopCore.Data.EF;
using OnlineShopCore.Data.Entities;

namespace OnlineShopCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductCategoriesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductCategoriesApi
        [HttpGet]
        public IEnumerable<ProductCategory> GetProductCategories()
        {
            return _context.ProductCategories;
        }

        // GET: api/ProductCategoriesApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }

        // PUT: api/ProductCategoriesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory([FromRoute] int id, [FromBody] ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategoriesApi
        [HttpPost]
        public async Task<IActionResult> PostProductCategory([FromBody] ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.Id }, productCategory);
        }

        // DELETE: api/ProductCategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();

            return Ok(productCategory);
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}