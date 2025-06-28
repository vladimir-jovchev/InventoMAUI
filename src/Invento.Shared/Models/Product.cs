using System.ComponentModel.DataAnnotations;
namespace Invento.Shared.Models;

public class Product
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public string? Sku { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int BusinessId { get; set; }
    public Business Business { get; set; } = null!;
    public decimal Cost { get; set; }
    public decimal? SellingPrice { get; set; }
    [Required] public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal MinimumStock { get; set; }
    public decimal ReorderPoint { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
}
