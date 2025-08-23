using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyApiProject.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        public string CustomerId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
