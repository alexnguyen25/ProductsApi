using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;
namespace ProductsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase{
    
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context) {
         _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber=1, [FromQuery] int pageSize=20) {
        List<Category> categoriesList = await _context.Categories.Where(c => c.IsDeleted == false)
        .Include(c => c.Products.Where(p => !p.IsDeleted))
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return Ok(categoriesList);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id) {
        Category? category = await _context.Categories.Where(c => c.IsDeleted == false && c.Id == id).Include(c => c.Products.Where(p => !p.IsDeleted)).FirstOrDefaultAsync();

        if(category != null) {
            return Ok(category);
        }
        else {
            return NotFound();
        }
    }

    [HttpPost] // Create
    public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category) {
        category.IsDeleted = false;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")] // Update
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] Category updatedCategory) {
        if (id != updatedCategory.Id){
        return BadRequest("Category ID mismatch");
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound($"Product with Id = {id} not found");
        }

        category.Name = updatedCategory.Name;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")] // Delete
    public async Task<IActionResult> DeleteCategory([FromRoute] int id) {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.IsDeleted == true) {
            return NotFound();
        }

        category.IsDeleted = true;

        List<Product> products = await _context.Products.Where(p => p.CategoryId == id && p.IsDeleted != true).ToListAsync();
        foreach (Product product in products) {
            product.IsDeleted = true;
            
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
}