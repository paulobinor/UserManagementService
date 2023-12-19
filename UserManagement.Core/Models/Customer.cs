using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.Models
{
    // Models/Customer.cs
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}