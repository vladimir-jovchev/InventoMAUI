using Microsoft.EntityFrameworkCore;
using Invento.Shared.Models;

namespace Invento.Api.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Business> Businesses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<StockMovement> StockMovements { get; set; }
		public DbSet<InventoryLevel> InventoryLevels { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed data for Business
			modelBuilder.Entity<Business>().HasData(new Business
			{
				Id = 1,
				Name = "Default Business",
				Email = "default@business.com",
				PhoneNumber = "123-456-7890",
				Address = "123 Main St",
				TaxId = "12345",
				Type = BusinessType.Retail,
				CreatedAt = DateTime.UtcNow
			});

			// Configure relationships
			modelBuilder.Entity<User>()
				.HasOne(u => u.Business)
				.WithMany(b => b.Users)
				.HasForeignKey(u => u.BusinessId)
				.IsRequired(false);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Business)
				.WithMany(b => b.Products)
				.HasForeignKey(p => p.BusinessId)
				.IsRequired(false);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryId)
				.IsRequired(false);
		}
	}
}
