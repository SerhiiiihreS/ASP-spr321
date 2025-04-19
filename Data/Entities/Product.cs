using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP_spr321.Data.Entities
{
    public record Product
    {
        public Guid    Id { get; set; }
        public Guid   CategoryId { get; set; }
        public String  Name { get; set; } = null!;
        public String? Description { get; set; } = null!;
        public String? Slug { get; set; } = null!;
        public String  ImagesCsv { get; set; } = String.Empty;
        public DateTime? DeletedAt { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; set; } 
        public int     Stock { get; set; } = 1;

        [JsonIgnore]
        public Category Category { get; set; } = null!;
    }
}
