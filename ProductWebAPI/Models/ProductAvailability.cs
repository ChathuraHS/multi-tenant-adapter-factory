using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductWebAPI.Models
{
    [Table("product_availabilities", Schema = "db")]
    public class ProductAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("available_units")]
        public int AvailableUnits { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
    }

}
