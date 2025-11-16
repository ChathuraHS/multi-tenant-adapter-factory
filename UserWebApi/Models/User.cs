using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserWebAPI.Models
{
    [Table("customer", Schema = "dbo")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int Id { get; set; }

        [Column("user_name")]
        public string Name { get; set; }

        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("password_hash")]
        [Required]
        public string PasswordHash { get; set; }

        [Column("role")]
        public string Role { get; set; }


    }
}
