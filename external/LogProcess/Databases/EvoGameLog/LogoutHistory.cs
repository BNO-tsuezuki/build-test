using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("logout_history")]
    public class LogoutHistory
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("datetime", TypeName = "datetime(6)")]
        [Required]
        public DateTime Datetime { get; set; }

        [Column("player_id", TypeName = "bigint")]
        [Required]
        public long PlayerId { get; set; }

        [Column("remote_ip", TypeName = "varchar(45)")]
        [Required]
        public string RemoteIp { get; set; }
    }
}
