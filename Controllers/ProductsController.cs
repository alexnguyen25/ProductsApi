using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;
namespace ProductsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase{
    
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context) {
         _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber=1, [FromQuery] int pageSize=20) {
        List<Product> productList = await _context.Products.Where(p => p.IsDeleted == false)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return Ok(productList);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id) {
        Product? product = await _context.Products.Where(p => p.IsDeleted == false && p.Id == id).FirstOrDefaultAsync();

        if(product != null) {
            return Ok(product);
        }
        else {
            return NotFound();
        }
    }

    [HttpPost] // Create
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product) {
        product.IsDeleted = false;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")] // Update
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] Product updatedProduct) {
        if (id != updatedProduct.Id){
        return BadRequest("Product ID mismatch");
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound($"Product with Id = {id} not found");
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.CategoryId = updatedProduct.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")] // Delete
    public async Task<IActionResult> DeleteProduct([FromRoute] int id) {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.IsDeleted == true) {
            return NotFound();
        }

        product.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }


}