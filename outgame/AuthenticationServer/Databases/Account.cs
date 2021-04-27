using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Databases
{
    public class Account
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]//値の自動生成なし
		public string account { get; set; }

		[Required]
		public string hashedPassword { get; set; }

		[Required]
		public string salt { get; set; }

		[Required]
		public int permission { get; set; }

		[Required]
		public string nickname { get; set; }

		[Required]
		public evolib.HostType hostType { get; set; }
	}
}
