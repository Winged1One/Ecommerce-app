using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.Repositories;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand, type, sort));
    }
    [HttpGet("{id:int}")]  //api/Products/2
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        if(id <= 0) return BadRequest();
        var product = await repo.GetProductByIdAsync(id);

        if(product == null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProductById", new {id = product.Id}, product);
        }
        return BadRequest("Problem creating product");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if(product.Id !=id || !ProductExist(id))
            return BadRequest("Cannot update the product");
        
        repo.UpdateProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem in updating the product");
    }
    private bool ProductExist(int id)
    {
        return repo.ProductExist(id);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        if(!ProductExist(id))
            return BadRequest();

        var product = await repo.GetProductByIdAsync(id);
        if(product == null ) return NotFound();
        
        repo.DeleteProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem in deleting the product");
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypeAsync());
    }
}