using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;

using Amazon.S3;
using Amazon.S3.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using evolib;
using evolib.Kvs.Models;
using evolib.Services.MasterData;
using evotool.ProtocolModels.MasterData;
using evolib.DeliveryData;
using evolib.Databases.common2;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MasterDataController : BaseController
	{
		public MasterDataController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		class DummyRequester : evolib.Util.HttpRequester
		{
			public override string Path { get { return ""; } }

			static HttpClient _httpClient = new HttpClient();
			protected override HttpClient HttpClient { get { return _httpClient; } }
		}

		[HttpPost]
		public async Task<IActionResult> SetPath([FromBody]SetPath.Request req)
		{
			try
			{
				var requester = new DummyRequester();

				var plainPath = req.path;
				var resPlain = await requester.GetAsync(plainPath);
				if (resPlain.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return BadRequest("Not Found: plain masterdata.");
				}

				var encryptPath = req.path.Substring(0, req.path.Length - Path.GetExtension(req.path).Length);
				var resEncrypt = await requester.GetBinaryAsync(encryptPath);
				if (resEncrypt.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return BadRequest("Not Found: encryption masterdata.");
				}

				var s3PutClient = new AmazonS3Client(
					Amazon.RegionEndpoint.GetBySystemName(DeliveryDataInfo.S3BucketRegion)
				);

				var plainKey = evolib.Util.KeyGen.GetUrlSafe(64);
				{
					var putReq = new PutObjectRequest()
					{
						BucketName = DeliveryDataInfo.S3BucketName,

						Key = plainKey,

						InputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(resPlain.Payload)),
					};
					var putRes = await s3PutClient.PutObjectAsync(putReq);
				}

				var encryptkey = evolib.Util.KeyGen.GetUrlSafe(32);
				{
					var putReq = new PutObjectRequest()
					{
						BucketName = DeliveryDataInfo.S3BucketName,

						Key = encryptkey,

						InputStream = new MemoryStream(resEncrypt.Payload),
					};
					var putRes = await s3PutClient.PutObjectAsync(putReq);
				}


				var masterData = await MasterDataLoader.ParseAsync(resPlain.Payload, "", "" );

				var masterDataPath = new MasterDataPath();
				masterDataPath.Model.pathSrc = req.path;
				masterDataPath.Model.s3KeyPlain = plainKey;
				masterDataPath.Model.pathPlain = $"https://{DeliveryDataInfo.CfDomainName }/{plainKey}";
				masterDataPath.Model.pathEncrypt = $"https://{DeliveryDataInfo.CfDomainName }/{encryptkey}";
				masterDataPath.Model.updateDate = System.DateTime.UtcNow;
				masterDataPath.Model.version = masterData.VersionStr;
				await masterDataPath.SaveAsync();


				System.Action<GenericData> pushRecord = (record) =>
				{
					record.MasterDataPathRecord(
						req.path,
						plainKey,
						$"https://{DeliveryDataInfo.CfDomainName }/{plainKey}",
						$"https://{DeliveryDataInfo.CfDomainName }/{encryptkey}",
						masterData.VersionStr
					);
				};
				var rec = await Common2DB.GenericDatas.FindAsync(GenericData.Type.MasterDataPath);
				if (rec == null)
				{
					rec = new GenericData();
					pushRecord(rec);
					await Common2DB.GenericDatas.AddAsync(rec);
				}
				else
				{
					pushRecord(rec);
				}
				await Common2DB.SaveChangesAsync();


				var res = new SetPath.Response();
				return Ok(res);
			}
			catch (System.Exception e)
			{
				return BadRequest("Failed to update.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> SetPath(string path, string pathEncrypt)
		{
			try
			{
				if (string.IsNullOrEmpty(pathEncrypt))
				{
					pathEncrypt = path.Substring(0, path.Length - Path.GetExtension(path).Length);
				}

				var requester = new DummyRequester();
				var response = await requester.GetAsync(pathEncrypt);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return BadRequest("param: pathEncrypt is invalid.");
				}
				response = await requester.GetAsync(path);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return BadRequest("param: path is invalid.");
				}

				var masterData = await MasterDataLoader.ParseAsync(response.Payload, "", "");

				var masterDataPath = new MasterDataPath();
				masterDataPath.Model.pathSrc = path;
				masterDataPath.Model.s3KeyPlain = "";
				masterDataPath.Model.pathPlain = path;
				masterDataPath.Model.pathEncrypt = pathEncrypt;
				masterDataPath.Model.updateDate = System.DateTime.UtcNow;
				masterDataPath.Model.version = masterData.VersionStr;
				await masterDataPath.SaveAsync();


				System.Action<GenericData> pushRecord = (record) =>
				{
					record.MasterDataPathRecord(
						path,
						"",
						path,
						pathEncrypt,
						masterData.VersionStr
					);
				};
				var rec = await Common2DB.GenericDatas.FindAsync(GenericData.Type.MasterDataPath);
				if (rec == null)
				{
					rec = new GenericData();
					pushRecord(rec);
					await Common2DB.GenericDatas.AddAsync(rec);
				}
				else
				{
					pushRecord(rec);
				}
				await Common2DB.SaveChangesAsync();
			}
			catch(System.Exception ex)
			{
				return BadRequest(ex.Message);
			}

			var res = new SetPath.Response();
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetVersion([FromBody]GetVersion.Request req)
		{
			var masterDataPath = new MasterDataPath();
			await masterDataPath.FetchAsync();

			return Ok(new GetVersion.Response
			{
				masterDataVersion = masterDataPath.Model.version,
			});
		}
	}
}
