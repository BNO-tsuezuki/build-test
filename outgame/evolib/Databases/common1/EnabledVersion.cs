using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common1
{
	public class EnabledVersion
	{
		// key1
		public VersionChecker.CheckTarget checkTarget { get; set; }

		// key2
		public VersionChecker.ReferenceSrc referenceSrc { get; set; }


		[Required]
		public int major { get; set; }
		[Required]
		public int minor { get; set; }
		[Required]
		public int patch { get; set; }
		[Required]
		public int build { get; set; }



		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime updateDate { get; set; }
	}
}
