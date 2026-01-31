using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiProject.Models
{
    /// <summary>
    /// Represents an API key in the khhapikey table.
    /// </summary>
    [Table("khhapikey")]
    public class ApiKey
    {
        /// <summary>
        /// Gets or sets the API key string.
        /// </summary>
        [Key]
        [Column("apikey")]
        public string? Key { get; set; }

        public Guid Id { get; set; } // 確保這裡有 Id 屬性


        [Required]
        [StringLength(50)]
        [Column("developer")]
        public string Developer { get; set; }

        [Column("createdate")]
        public DateTime? CreateDate { get; set; }
    }
}
