using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("player_exp_history")]
    public class PlayerExpHistory
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

        [Column("exp", TypeName = "int")]
        [Required]
        public int Exp { get; set; }

        [Column("total_exp", TypeName = "int")]
        [Required]
        public int TotalExp { get; set; }

        [Column("level", TypeName = "int")]
        [Required]
        public int Level { get; set; }
    }
}
