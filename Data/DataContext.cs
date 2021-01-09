using Microsoft.EntityFrameworkCore;
using dotnet_ef_api.Models;

namespace dotnet_ef_api.Data
{
  public class DataContext : DbContext
  {
      public DataContext(DbContextOptions<DataContext> options) : base (options) { }        
      public DbSet<Category> Categories { get; set; }
      public DbSet<Product> Products { get; set; }
  }
}