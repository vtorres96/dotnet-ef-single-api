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
			try
			{
        var products = await context.Products.Include(x => x.Category).ToListAsync();
        return products;
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
    {
      try 
      {
        var product = await context.Products.Include(x => x.Category)
          .AsNoTracking()
          .FirstOrDefaultAsync(x => x.Id == id);
        return product;
      }
			catch (Exception ex)
			{
					return BadRequest($"Error: {ex.Message}");
			}
    }

    [HttpGet]
    [Route("categories/{id}")]
    public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
    {
			try
			{
        var products = await context.Products.Include(x => x.Category)
          .AsNoTracking()
          .Where(x => x.Category.Id == id)
          .ToListAsync();
        return products;
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
    }

    [HttpPost]
		public async Task<ActionResult<Product>> Post(
				[FromServices] DataContext context, 
				[FromBody] Product model)
		{
			try
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
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

    [HttpPut("{id}")]
		public async Task<IActionResult> Put(
				int id, 
				[FromServices] DataContext context, 
				[FromBody] Product model)
		{
			try
			{
				var product = await context.Products.AsNoTracking()
					.Where(product => product.Id == id)
					.FirstOrDefaultAsync();
				if(product == null) return NotFound("Product not found");

				context.Products.Update(model);
				await context.SaveChangesAsync();
				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

    [HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id, [FromServices] DataContext context)
		{
			try
			{
				var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);
				if(product == null) return NotFound("Product not found");

				context.Remove(product);
				await context.SaveChangesAsync();
				return Ok(new { message = "Product deleted" });
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
  }
}