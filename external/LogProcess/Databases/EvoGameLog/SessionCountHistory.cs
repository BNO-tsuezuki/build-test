using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("session_count_history")]
    public class SessionCountHistory
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("area_name", TypeName = "varchar(200)")]
        [Required]
        public string AreaName { get; set; }

        [Column("datetime", TypeName = "datetime(6)")]
        [Required]
        public DateTime Datetime { get; set; }

        [Column("count", TypeName = "int")]
        [Required]
        public int Count { get; set; }
    }
}
