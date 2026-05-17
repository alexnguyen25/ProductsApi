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
    public async Task<IActionResult> GetAllProducts() {
        List<Product> productList = await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();

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

}