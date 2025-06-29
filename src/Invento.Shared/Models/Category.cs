using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public DateTime CreatedAt { get; set; } // Changed from CreatedDate
        public DateTime? ModifiedAt { get; set; } // Changed from ModifiedDate
        
        // Navigation properties
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> Subcategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
        public Category()
        {
            Subcategories = new HashSet<Category>();
            Products = new HashSet<Product>();
            CreatedAt = DateTime.UtcNow;
        }
    }
}