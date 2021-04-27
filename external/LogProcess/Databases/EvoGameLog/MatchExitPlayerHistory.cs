using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("match_exit_player_history")]
    public class MatchExitPlayerHistory
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

        // todo: 表示用にenum？
        [Column("match_format", TypeName = "int")]
        [Required]
        public int MatchFormat { get; set; }

        // todo: length
        [Column("match_id", TypeName = "varchar(43)")]
        [Required]
        public string MatchId { get; set; }

        // todo: 表示用にenum？
        [Column("match_exit_reason", TypeName = "int")]
        [Required]
        public int MatchExitReason { get; set; }
    }
}
