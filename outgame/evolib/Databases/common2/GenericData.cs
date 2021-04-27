using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common2
{
	public class GenericData
	{
		public enum Type
		{
			TranslationTablePath,
			MasterDataPath,
			MinMatchmakeEntriedPlayersNumber,
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Type type  { get; set; }

		public string data1 { get; set; }
		public string data2 { get; set; }
		public string data3 { get; set; }
		public string data4 { get; set; }
		public string data5 { get; set; }
		public string data6 { get; set; }
		public string data7 { get; set; }
		public string data8 { get; set; }
		

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime timeStamp { get; set; }
	}
}
