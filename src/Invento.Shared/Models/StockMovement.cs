using System;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class StockMovement
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime MovementDate { get; set; }
        public MovementType Type { get; set; }
        public string Notes { get; set; }
        public int? BusinessId { get; set; }
        public int? UserId { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string ReferenceNumber { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Business Business { get; set; }
        public virtual User User { get; set; }
    }
    
    public enum MovementType
    {
        Purchase = 1,
        Sale = 2,
        Return = 3,
        Adjustment = 4,
        Transfer = 5,
        StockCount = 6,
        Write_Off = 7
    }
}