//using System;
//using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//using Amazon.S3;
//using Amazon.S3.Model;

//using evolib.Databases.gamedb;
//using evoapi.ProtocolModels;
//using evoapi.ProtocolModels.Diorama;



//namespace evoapi.Controllers
//{
//	[Route("api/bR7cqQt5zsZd7dum/[action]")]
//	public class DioramaController : BaseController
//	{
//		static string BaseUrl = "https://d1m0wg1e6o8rn8.cloudfront.net/";
//		static string BucketName = "bno-evo-diorama2";

		

//		public DioramaController(
//			GameDBContext gameDb
//			, Services.IServicePack servicePack
//		):base(gameDb, servicePack)
//		{
//		}

//        [HttpPost]
//		public async Task<IActionResult> UploadVisual([FromForm]UploadVisual.Request req)
//		{
//			if ((int)evolib.Diorama.Const.SceneDataLimitSize < req.sceneData.Length)
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Msg, Error.ErrorCode.DioramaSceneDataSizeOver);
//			}
//			if ((int)evolib.Diorama.Const.VisualLimitSize < req.visual.Length)
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Msg, Error.ErrorCode.DioramaVisualSizeOver);
//			}

//			var count = await GameDb.DioramaUploadedVisuals.CountAsync(r => r.playerId == SelfHost.playerInfo.playerId);
//			if ((int)evolib.Diorama.Const.UploadVisualMax <= count )
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Logout, Error.ErrorCode.BadRequest);
//			}

//			if(await GameDb.DioramaUploadedVisuals.AnyAsync(r =>
//					   r.playerId == SelfHost.playerInfo.playerId && r.hashCode == req.hashCode))
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Logout, Error.ErrorCode.BadParameter);
//			}

//			try
//			{
//				var sceneDataStream = new MemoryStream();
//				await req.sceneData.CopyToAsync(sceneDataStream);

//				var visualStream = new MemoryStream();
//				await req.visual.CopyToAsync(visualStream);

//				//190613 サーバー側で画像データ確認はやらない。というかやっても無駄。
//				//var img = Image.FromStream( visualStream, false, true);
//				//if (!img.RawFormat.Equals(ImageFormat.Jpeg))
//				//{
//				//	return BuildErrorResponse(Error.ErrorLevel.Logout, Error.ErrorCode.DioramaUploadedIllegalFormat);
//				//}

//				var s3Client = new AmazonS3Client(
//					//"", "",
//					Amazon.RegionEndpoint.APNortheast1
//				);


//				var directory = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
//								+ "/" + SelfHost.playerInfo.playerId
//								+ "/visuals/"
//								+ evolib.Util.KeyGen.GetUrlSafe(32);

//				var visualKey = directory + "/visual/" + evolib.Util.KeyGen.GetUrlSafe(16);
//				var sceneDataKey = directory + "/sceneData/" + evolib.Util.KeyGen.GetUrlSafe(16); ;

//				var putReqVisual = new PutObjectRequest()
//				{
//					BucketName = BucketName,

//					Key = visualKey,

//					InputStream = visualStream,
//				};
//				var putResVisual = await s3Client.PutObjectAsync(putReqVisual);

//				var putReqSceneData = new PutObjectRequest()
//				{
//					BucketName = BucketName,

//					Key = sceneDataKey,

//					InputStream = sceneDataStream,
//				};
//				var putResSceneData = await s3Client.PutObjectAsync(putReqSceneData);


//				await GameDb.DioramaUploadedVisuals.AddAsync(
//					new DioramaUploadedVisual()
//					{
//						playerId = SelfHost.playerInfo.playerId,
//						hashCode = req.hashCode,
//						visualKey = visualKey,
//						sceneDataKey = sceneDataKey,
//						timeStamp = DateTime.UtcNow,
//					}
//				);
//				await GameDb.SaveChangesAsync();

//				var res = new UploadVisual.Response();
//				return Ok(res);
//			}
//			catch (ArgumentException)
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Msg, Error.ErrorCode.SeverInternalError);
//			}
//		}

//		[HttpPost]
//		public async Task<IActionResult> CancelUploadedVisual([FromBody]CancelUploadedVisual.Request req)
//		{
//			var visual = await GameDb.DioramaUploadedVisuals.FindAsync(req.id);
//			if (visual != null && visual.playerId == SelfHost.playerInfo.playerId )
//			{
//				GameDb.DioramaUploadedVisuals.Remove(visual);
//				await GameDb.SaveChangesAsync();
//			}

//			var res = new CancelUploadedVisual.Response();
//			return Ok(res);
//		}

//		[HttpPost]
//		public async Task<IActionResult> GetUploadedVisualUrls([FromBody]GetUploadedVisualUrls.Request req)
//		{
//			var uploadedVisuals = await GameDb.DioramaUploadedVisuals
//										.Where(r => r.playerId == SelfHost.playerInfo.playerId)
//										.ToListAsync();

//			var res = new GetUploadedVisualUrls.Response();
//			res.urls = new List<GetUploadedVisualUrls.Response.Url>();

//			for ( int i=0; i<uploadedVisuals.Count; i++ )
//			{
//				var rec = uploadedVisuals[i];
//				res.urls.Add(new GetUploadedVisualUrls.Response.Url()
//				{
//					id = rec.Id,
//					hashCode = rec.hashCode,
//					sceneDataUrl = BaseUrl + rec.sceneDataKey,
//					visualUrl = BaseUrl + rec.visualKey,
//				});
//			}

//			return Ok(res);
//		}



//		[HttpPost]
//		public async Task<IActionResult> SaveSceneData([FromForm]SaveSceneData.Request req)
//		{
//			if( (int)evolib.Diorama.Const.SceneDataLimitSize < req.sceneData.Length )
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Msg, Error.ErrorCode.DioramaSceneDataSizeOver);
//			}

//			try
//			{
//				var sceneDataStream = new MemoryStream();
//				await req.sceneData.CopyToAsync(sceneDataStream);
				

//				var s3Client = new AmazonS3Client(
//					//"", "",
//					Amazon.RegionEndpoint.APNortheast1
//				);

//				var directory = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
//								+ "/" + SelfHost.playerInfo.playerId
//								+ "/sceneDatas/"
//								+ req.index.Value;

//				var filename = evolib.Util.KeyGen.GetUrlSafe(32);

//				var sceneDataKey = directory + "/" + filename;

//				var putReqSceneData = new PutObjectRequest()
//				{
//					BucketName = BucketName,

//					Key = sceneDataKey,

//					InputStream = sceneDataStream,
//				};
//				var putResSceneData = await s3Client.PutObjectAsync(putReqSceneData);


//				var savedSceneData = await GameDb.DioramaSavedSceneDatas.FindAsync(
//												SelfHost.playerInfo.playerId,
//												req.index);
//				if( savedSceneData == null)
//				{
//					await GameDb.DioramaSavedSceneDatas.AddAsync(new DioramaSavedSceneData()
//					{
//						playerId = SelfHost.playerInfo.playerId,
//						index = req.index.Value,
//						hashCode = req.hashCode,
//						sceneDataKey = sceneDataKey,
//						timeStamp = DateTime.UtcNow,
//					});
//					await GameDb.SaveChangesAsync();
//				}
//				else
//				{
//					var oldSceneDataKey = savedSceneData.sceneDataKey;

//					savedSceneData.hashCode = req.hashCode;
//					savedSceneData.sceneDataKey = sceneDataKey;
//					savedSceneData.timeStamp = DateTime.UtcNow;
//					await GameDb.SaveChangesAsync();

//					var deleteReq = new DeleteObjectRequest()
//					{
//						BucketName = BucketName,

//						Key = oldSceneDataKey,
//					};
//					await s3Client.DeleteObjectAsync(deleteReq);
//				}

//				var res = new SaveSceneData.Response();
//				return Ok(res);
//			}
//			catch (ArgumentException)
//			{
//				return BuildErrorResponse(Error.ErrorLevel.Msg, Error.ErrorCode.SeverInternalError);
//			}
//		}

//		[HttpPost]
//		public async Task<IActionResult> GetSavedSceneDataUrls([FromBody]GetSavedSceneDataUrls.Request req)
//		{
//			var savedSceneDatas = await GameDb.DioramaSavedSceneDatas
//										.Where(r => r.playerId == SelfHost.playerInfo.playerId)
//										.ToListAsync();


//			var res = new GetSavedSceneDataUrls.Response();
//			res.urls = new List<GetSavedSceneDataUrls.Response.Url>();

//			for ( int i = 0; i<savedSceneDatas.Count; i++)
//			{
//				var rec = savedSceneDatas[i];

//				res.urls.Add(new GetSavedSceneDataUrls.Response.Url()
//				{
//					index = rec.index,
//					sceneDataUrl = BaseUrl + rec.sceneDataKey,
//					hashCode = rec.hashCode,
//				});
//			}

//			return Ok(res);
//		}
//	}
//}
