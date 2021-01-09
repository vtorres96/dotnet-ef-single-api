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
				var categories = await context.Categories.ToListAsync();
				return categories;
		}

		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
				return "value";
		}

		[HttpPost]
		public async Task<ActionResult<Category>> Post(
				[FromServices] DataContext context, 
				[FromBody] Category model)
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