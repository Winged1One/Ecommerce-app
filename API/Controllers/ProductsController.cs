using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _context;
    public ProductsController(StoreContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }
    [HttpGet("{id:int}")]  //api/Products/2
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        if(id <= 0) return BadRequest();
        var product = await _context.Products.FindAsync(id);

        if(product == null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
         _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if(product.Id !=id || !ProductExist(id))
            return BadRequest("Cannot update the product");
        
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    private bool ProductExist(int id)
    {
        return _context.Products.Any(x => x.Id == id);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        if(!ProductExist(id))
            return BadRequest();

        var product = await _context.Products.FindAsync(id);
        if(product == null ) return NotFound();
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}