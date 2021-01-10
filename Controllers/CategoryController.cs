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
	[Route("api/categories")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
		{
			try
			{
				var categories = await context.Categories.ToListAsync();
				return Ok(categories);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> GetById([FromServices] DataContext context, int id)
		{
			try
			{
				var category = await context.Categories
					.AsNoTracking()
					.FirstOrDefaultAsync(category => category.Id == id);
        return Ok(category);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

		[HttpPost]
		public async Task<ActionResult<Category>> Post(
				[FromServices] DataContext context, 
				[FromBody] Category model)
		{
			try
			{
				if(ModelState.IsValid)
				{
					context.Categories.Add(model);
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
				[FromBody] Category model)
		{
				try
				{
					var category = await context.Categories.AsNoTracking()
						.Where(category => category.Id == id)
						.FirstOrDefaultAsync();
					if(category == null) return NotFound("Category not found");

					context.Categories.Update(model);
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
				var category = await context.Categories.FirstOrDefaultAsync(category => category.Id == id);
				if(category == null) return NotFound("Category not found");

				context.Remove(category);
				await context.SaveChangesAsync();
				return Ok(new { message = "Category deleted" });
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
	}
}