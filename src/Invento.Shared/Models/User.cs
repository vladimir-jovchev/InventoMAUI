using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invento.Shared.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public int? BusinessId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } // Changed from CreatedDate
        public DateTime? LastLoginDate { get; set; }
        
        // Navigation property
        public virtual Business Business { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; }
        
        public User()
        {
            StockMovements = new HashSet<StockMovement>();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}