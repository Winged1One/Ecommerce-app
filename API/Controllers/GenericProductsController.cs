using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Entities;
using Core.Specifications;
using API.RequestHelpers;
namespace API.Controllers;

public class GenericProductsController(IGenericRepository<Product> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery]ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }
    [HttpGet("{id:int}")]  //api/Products/2
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        if(id <= 0) return BadRequest();
        var product = await repo.GetByIdAsync(id);

        if(product == null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync())
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
        
        repo.Update(product);
        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem in updating the product");
    }
    private bool ProductExist(int id)
    {
        return repo.Exist(id);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        if(!ProductExist(id))
            return BadRequest();

        var product = await repo.GetByIdAsync(id);
        if(product == null ) return NotFound();
        
        repo.Remove(product);
        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem in deleting the product");
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }
}