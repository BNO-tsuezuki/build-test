using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.EntityFrameworkCore;

using evolib.Databases.common2;


namespace evotool
{
	public static class TranslationTable
	{
		public static string SrcPath { get; set; }
		public static string RawData { get; set; }


		public static void TranslationTableRecord( this GenericData rec, string path)
		{
			rec.type = GenericData.Type.TranslationTablePath;
			rec.data1 = path;
		}


		class DummyRequester : evolib.Util.HttpRequester
		{
			public override string Path { get { return ""; } }

			static HttpClient _httpClient = new HttpClient();
			protected override HttpClient HttpClient { get { return _httpClient; } }
		}

		public static async Task LoadAsync(Common2DBContext db )
		{
			var rec = await db.GenericDatas
				.Where(r => r.type == GenericData.Type.TranslationTablePath)
				.FirstOrDefaultAsync();

			if (rec == null) return;

			var requester = new DummyRequester();

			var res = await requester.GetAsync(rec.data1);
			if (res.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return;
			}

			SrcPath = rec.data1;
			RawData = res.Payload;
		}
	}
}
