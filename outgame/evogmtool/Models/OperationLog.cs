using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evogmtool.Models
{
    [Table("OperationLogs")]
    public class OperationLog
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("userId")]
        public int? UserId { get; set; }

        [Column("statusCode", TypeName = "smallint(6)")]
        [Required]
        public short StatusCode { get; set; }

        [Column("method", TypeName = "varchar(6)")]
        [Required]
        [MaxLength(6)]
        public string Method { get; set; }

        [Column("url", TypeName = "tinytext")]
        [Required]
        public string Url { get; set; }

        [Column("queryString", TypeName = "text")]
        public string QueryString { get; set; }

        [Column("requestBody", TypeName = "mediumtext")]
        public string RequestBody { get; set; }

        [Column("responseBody", TypeName = "mediumtext")]
        public string ResponseBody { get; set; }

        [Column("exception", TypeName = "mediumtext")]
        public string Exception { get; set; }

        [Column("ipAddress", TypeName = "varchar(45)")]
        [Required]
        [MaxLength(45)]
        public string IpAddress { get; set; }

        [Column("createdAt", TypeName = "datetime(6)")]
        [Required]
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
