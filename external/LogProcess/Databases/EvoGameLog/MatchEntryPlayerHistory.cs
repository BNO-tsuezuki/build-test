using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("match_entry_player_history")]
    public class MatchEntryPlayerHistory
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

        [Column("match_entry_reason", TypeName = "int")]
        [Required]
        public int MatchEntryReason { get; set; }
    }
}
