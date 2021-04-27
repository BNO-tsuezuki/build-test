using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("player_account_create_history")]
    public class PlayerAccountCreateHistory
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

        // todo: length
        [Column("player_name", TypeName = "varchar(32)")]
        [Required]
        public string PlayerName { get; set; }

        // todo: 表示用にenumとか必要？
        [Column("account_type", TypeName = "int")]
        [Required]
        public int AccountType { get; set; }
    }
}
