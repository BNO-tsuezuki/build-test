using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evogmtool.Models
{
    [Table("AuthLogs")]
    public class AuthLog
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("account", TypeName = "varchar(254)")]
        [Required]
        [MaxLength(254)]
        public string Account { get; set; }

        [Column("result", TypeName = "tinyint(4)")]
        [Required]
        public sbyte Result { get; set; }

        [Column("ipAddress", TypeName = "varchar(45)")]
        [Required]
        [MaxLength(45)]
        public string IpAddress { get; set; }

        [Column("createdAt", TypeName = "datetime(6)")]
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
