using Invento.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Invento.Infrastructure.Data;

public class InventoDbContext : DbContext {
	public InventoDbContext(DbContextOptions<InventoDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Business> Businesses { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<StockMovement> StockMovements { get; set; }
	public DbSet<InventoryLevel> InventoryLevels { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		// Configure entities with proper relationships
		modelBuilder.Entity<User>(entity => {
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => e.Email).IsUnique();
			entity.Property(e => e.Email).IsRequired();
		});

		modelBuilder.Entity<Product>(entity => {
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired();
			entity.HasOne(e => e.Business).WithMany().HasForeignKey(e => e.BusinessId);
			entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
		});

		// Seed initial data
		SeedData(modelBuilder);
	}

	private static void SeedData(ModelBuilder modelBuilder) {
		// Default business
		modelBuilder.Entity<Business>().HasData(new Business {
			Id = 1,
			Name = "Demo Coffee Shop",
			Type = BusinessType.Service,
			Address = "123 Main St",
			CreatedAt = DateTime.UtcNow
		});

		// Default categories
		modelBuilder.Entity<Category>().HasData(
			new Category { Id = 1, Name = "Beverages", CreatedAt = DateTime.UtcNow },
			new Category { Id = 2, Name = "Food", CreatedAt = DateTime.UtcNow },
			new Category { Id = 3, Name = "Supplies", CreatedAt = DateTime.UtcNow }
		);

		// Default admin user
		modelBuilder.Entity<User>().HasData(new User {
			Id = 1,
			Email = "admin@invento.com",
			FirstName = "System",
			LastName = "Administrator",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
			Role = UserRole.Administrator,
			BusinessId = 1,
			CreatedAt = DateTime.UtcNow
		});
	}
}
