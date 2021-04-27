using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common1
{
	public class OpsNotice
	{
		[Key]
		public long Id { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime updateDate { get; set; }
		public int version { get; set; }



		[Required]
		public bool release { get; set; }

		[Required]
		public UInt64 target { get; set; }

		public string memo { get; set; }

		[Required]
		public DateTime beginDate { get; set; }
		[Required]
		public DateTime endDate { get; set; }

		public bool enabledEnglish { get; set; }
		public string msgEnglish { get; set; }

		public bool enabledFrench { get; set; }
		public string msgFrench { get; set; }

		public bool enabledGerman { get; set; }
		public string msgGerman { get; set; }

		public bool enabledJapanese { get; set; }
		public string msgJapanese { get; set; }



		[Required]
		public OptNoticeType optNoticeType { get; set; }


		// for OptNoticeType.Chat
		[Required]
		public int times { get; set; }
		public int repeateIntervalMinutes { get; set; }


		// for OptNoticeType.Popup
		public string titleEnglish { get; set; }

		public string titleFrench { get; set; }

		public string titleGerman { get; set; }

		public string titleJapanese { get; set; }

        // for OptNoticeType.Topics
		[Required]
        public int priority { get; set; }
        public string redirectUI { get; set; }
	}
}
