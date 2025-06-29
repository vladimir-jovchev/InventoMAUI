using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string TaxId { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } // Changed from CreatedDate
        public DateTime? ModifiedAt { get; set; } // Changed from ModifiedDate
        
        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
        
        public Supplier()
        {
            Products = new HashSet<Product>();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }
}