using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class MutePlayer
	{
		public long playerIdSrc { get; set; }

		public long playerIdDst { get; set; }

		public bool text { get; set; }

		public bool voice { get; set; }

		public DateTime timeStamp { get; set; }
	}
}
