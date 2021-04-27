using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("chat_direct_history")]
    public class ChatDirectHistory
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

        [Column("text", TypeName = "nvarchar(96)")]
        [Required]
        public string Text { get; set; }

        [Column("target_player_id", TypeName = "bigint")]
        [Required]
        public long TargetPlayerId { get; set; }
    }
}
