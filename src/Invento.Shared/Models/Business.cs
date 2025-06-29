using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
	public class Business
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string TaxId { get; set; } = string.Empty;
		public BusinessType Type { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
        
		// Navigation properties
		public virtual ICollection<User> Users { get; set; }
		public virtual ICollection<Product> Products { get; set; }
        
		public Business()
		{
			Users = new HashSet<User>();
			Products = new HashSet<Product>();
			CreatedAt = DateTime.UtcNow;
		}
	}
}