using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("match_execution_history")]
    public class MatchExecutionHistory
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        // todo: 開始と終了どちらで見るか要確認
        [Column("datetime", TypeName = "datetime(6)")]
        [Required]
        public DateTime Datetime { get; set; }

        // todo: 表示用にenum？
        [Column("match_format", TypeName = "int")]
        [Required]
        public int MatchFormat { get; set; }

        // todo: length
        [Column("match_id", TypeName = "varchar(43)")]
        [Required]
        public string MatchId { get; set; }

        // todo: 表示用にenum？
        [Column("rule_format", TypeName = "int")]
        [Required]
        public int RuleFormat { get; set; }

        // todo: 表示用にenum？
        [Column("match_win_team", TypeName = "int")]
        [Required]
        public int MatchWinTeam { get; set; }

        // todo: 表示用にenum？
        [Column("match_lose_team", TypeName = "int")]
        [Required]
        public int MatchLoseTeam { get; set; }
    }
}
