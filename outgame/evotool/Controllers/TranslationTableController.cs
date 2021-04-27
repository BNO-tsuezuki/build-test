using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Amazon.S3;
using Amazon.S3.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using evolib;
using evolib.Kvs.Models;
using evotool.ProtocolModels.TranslationTable;
using evolib.Databases.common2;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class TranslationTableController : BaseController
	{
		public TranslationTableController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpPost]
		public async Task<IActionResult> SetPath([FromBody]SetPath.Request req)
		{
			var rec = await Common2DB.GenericDatas.FindAsync(GenericData.Type.TranslationTablePath);
			if( rec == null)
			{
				rec = new GenericData();
				rec.TranslationTableRecord(req.path);
				await Common2DB.GenericDatas.AddAsync(rec);
			}
			else
			{
				rec.TranslationTableRecord(req.path);
			}
			await Common2DB.SaveChangesAsync();

			await TranslationTable.LoadAsync(Common2DB);


			return Ok(req.path);
		}
	}
}
