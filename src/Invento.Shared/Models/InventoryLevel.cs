using System;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class InventoryLevel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BusinessId { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MaximumStock { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal AverageCost { get; set; }
        public InventoryStatus Status { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Business Business { get; set; }
    }
    
    public enum InventoryStatus
    {
        InStock = 1,
        LowStock = 2,
        OutOfStock = 3,
        Discontinued = 4,
        OnOrder = 5
    }
}