using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_ef_api.Data;
using dotnet_ef_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_ef_api.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
        var products = await context.Products.Include(x => x.Category).ToListAsync();
        return products;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
    {
        var product = await context.Products.Include(x => x.Category)
          .AsNoTracking()
          .FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }

    [HttpGet]
    [Route("categories/{id}")]
    public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
    {
        var products = await context.Products.Include(x => x.Category)
          .AsNoTracking()
          .Where(x => x.Category.Id == id)
          .ToListAsync();
        return products;
    }

    [HttpPost]
		public async Task<ActionResult<Product>> Post(
				[FromServices] DataContext context, 
				[FromBody] Product model)
		{
				if(ModelState.IsValid)
				{
						context.Products.Add(model);
						await context.SaveChangesAsync();
						return model;
				} else {
						return BadRequest(ModelState);
				}
		}

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}