using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("chat_say_history")]
    public class ChatSayHistory
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

        // todo: 表示用にenumとか必要？
        [Column("chat_type", TypeName = "int")]
        [Required]
        public int ChatType { get; set; }

        // todo: length
        [Column("group_id", TypeName = "varchar(44)")]
        [Required]
        public string GroupId { get; set; }

        // todo: length
        [Column("match_id", TypeName = "varchar(43)")]
        [Required]
        public string MatchId { get; set; }

        [Column("side", TypeName = "int")]
        [Required]
        public int Side { get; set; }

        [Column("text", TypeName = "nvarchar(96)")]
        [Required]
        public string Text { get; set; }
    }
}
