//
// 取得履歴の情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    public class GivenHistory
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long playerId { get; set; }

        [Required]
        public DateTime obtainedDate { get; set; }

        [Required]
        public evolib.GiveAndTake.Type type { get; set; }
        [MaxLength(32)]
        public string presentId { get; set; }
        public long amount { get; set; }

        [Required]
        public evolib.PresentBox.Type giveType { get; set; }
        [MaxLength(120)]
        public string text { get; set; }
    }
}