using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("match_start_player_history")]
    public class MatchStartPlayerHistory
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

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

        [Column("player_id_a_1", TypeName = "bigint")]
        public long? PlayerIdA1 { get; set; }

        [Column("player_id_a_2", TypeName = "bigint")]
        public long? PlayerIdA2 { get; set; }

        [Column("player_id_a_3", TypeName = "bigint")]
        public long? PlayerIdA3 { get; set; }

        [Column("player_id_a_4", TypeName = "bigint")]
        public long? PlayerIdA4 { get; set; }

        [Column("player_id_a_5", TypeName = "bigint")]
        public long? PlayerIdA5 { get; set; }

        [Column("player_id_a_6", TypeName = "bigint")]
        public long? PlayerIdA6 { get; set; }

        [Column("player_id_b_1", TypeName = "bigint")]
        public long? PlayerIdB1 { get; set; }

        [Column("player_id_b_2", TypeName = "bigint")]
        public long? PlayerIdB2 { get; set; }

        [Column("player_id_b_3", TypeName = "bigint")]
        public long? PlayerIdB3 { get; set; }

        [Column("player_id_b_4", TypeName = "bigint")]
        public long? PlayerIdB4 { get; set; }

        [Column("player_id_b_5", TypeName = "bigint")]
        public long? PlayerIdB5 { get; set; }

        [Column("player_id_b_6", TypeName = "bigint")]
        public long? PlayerIdB6 { get; set; }
    }
}
