using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductWebAPI.Models
{
    [Table("product", Schema = "db")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("external_id")]
        public string ExternalId { get; set; } // ID from external system ex- Bokun

        [Column("provider")]
        public string Provider { get; set; } //ex - Bokun

        [Column("product_type")]
        public string ProductType { get; set; } // ex - "Tour", "CarRental", "Hotel"

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; } //The foreign key.
        public Category Category { get; set; } //The navigation property that allows to access the full Category object.
        //Each product belongs to one Category.

        [Column("price")]
        public decimal Price { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Initialize collections
        public List<ProductImage> TourImages { get; set; } = new List<ProductImage>();
        public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
        public List<ProductAvailability> TourAvailabilities { get; set; } = new List<ProductAvailability>();


    }
}

//EF Core uses these fields to automatically understand how the tables are linked when creating the database or running queries.
