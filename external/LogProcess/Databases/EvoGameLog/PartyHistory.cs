using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogProcess.Databases.EvoGameLog
{
    [Table("party_history")]
    public class PartyHistory
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("datetime", TypeName = "datetime(6)")]
        [Required]
        public DateTime Datetime { get; set; }

        [Column("group_id", TypeName = "varchar(44)")]
        [Required]
        public string GroupId { get; set; }

        [Column("player_id", TypeName = "bigint")]
        [Required]
        public long PlayerId { get; set; }

        // todo: 表示用にenum？
        [Column("type", TypeName = "int")]
        [Required]
        public int Type { get; set; }

        [Column("player_id_1", TypeName = "bigint")]
        public long? PlayerId1 { get; set; }

        [Column("player_id_2", TypeName = "bigint")]
        public long? PlayerId2 { get; set; }

        [Column("player_id_3", TypeName = "bigint")]
        public long? PlayerId3 { get; set; }

        [Column("player_id_4", TypeName = "bigint")]
        public long? PlayerId4 { get; set; }

        [Column("player_id_5", TypeName = "bigint")]
        public long? PlayerId5 { get; set; }
    }
}
