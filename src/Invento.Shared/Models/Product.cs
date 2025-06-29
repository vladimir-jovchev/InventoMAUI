using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public int? CategoryId { get; set; }
        public int? BusinessId { get; set; }
        public int? SupplierId { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } // Changed from CreatedDate
        public DateTime? ModifiedAt { get; set; } // Changed from ModifiedDate
        
        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual Business Business { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; }
        public virtual ICollection<InventoryLevel> InventoryLevels { get; set; }
        
        public Product()
        {
            StockMovements = new HashSet<StockMovement>();
            InventoryLevels = new HashSet<InventoryLevel>();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }
}