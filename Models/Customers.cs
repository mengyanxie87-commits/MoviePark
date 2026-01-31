using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyApiProject.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        [MaxLength(5)]
        [Required]
        public string CustomerID { get; set; }

        [Required]
        [Column("CustomerName")]
        public string Name { get; set; } = string.Empty;

        [Column(name:"Address")]
        public string? Address { get; set; } = string.Empty;

        [Column(name:"Phone")]
        public string? Phone { get; set; } = string.Empty;

        [Required]
        [Column("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Column("City")]
        public string City { get; set; } = string.Empty;
    }
}
