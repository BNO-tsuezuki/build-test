using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evogmtool.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("userId")]
        [Required]
        public int UserId { get; set; }

        [Column("account", TypeName = "varchar(254)")]
        [Required]
        [MaxLength(254)]
        public string Account { get; set; }

        [Column("salt", TypeName = "varchar(32)")]
        [Required]
        [MaxLength(32)]
        public string Salt { get; set; }

        [Column("passwordHash", TypeName = "varchar(128)")]
        [Required]
        [MaxLength(128)]
        public string PasswordHash { get; set; }

        [Column("name", TypeName = "nvarchar(256)")]
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Column("role", TypeName = "varchar(20)")]
        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        [Column("publisherId", TypeName = "int")]
        [Required]
        public int PublisherId { get; set; }

        [Column("timezoneCode", TypeName = "varchar(100)")]
        [Required]
        public string TimezoneCode { get; set; }

        [Column("languageCode", TypeName = "char(2)")]
        [Required]
        public string LanguageCode { get; set; }

        [Column("isAvailable", TypeName = "bit(1)")]
        [Required]
        public bool IsAvailable { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("createdAt", TypeName = "datetime(6)")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("updatedAt", TypeName = "datetime(6)")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        public Publisher Publisher { get; set; }

        public Timezone Timezone { get; set; }

        public Language Language { get; set; }
    }
}
