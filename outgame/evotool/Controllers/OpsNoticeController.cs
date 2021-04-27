using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using evolib;
using evolib.Databases.common1;


using evotool.ProtocolModels.OpsNotice;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class OpsNoticeController : BaseController
	{
		public OpsNoticeController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		public async Task<IActionResult> Delete([FromBody]Delete.Request req)
		{
			var rec = await Common1DB.OpsNotices
				.Where(r => r.Id == req.id)
				.FirstOrDefaultAsync();
			if (rec == null) return BadRequest();


			Common1DB.OpsNotices.Remove(rec);
			await Common1DB.SaveChangesAsync();

			return Ok(new Delete.Response
			{
				deletedId = req.id,
			});
		}


		[HttpPost]
		public async Task<IActionResult> AddChat([FromBody]AddChat.Request req)
		{
			var newRec = new OpsNotice();
			newRec.Push(req.desc);
			
			await Common1DB.OpsNotices.AddAsync(newRec);
			await Common1DB.SaveChangesAsync();

			return Ok(new AddChat.Response
			{
				addedNotice = (ChatNotice)newRec,
			});
		}

		public async Task<IActionResult> EditChat([FromBody]EditChat.Request req)
		{
			var rec = await Common1DB.OpsNotices
				.Where(r => r.Id == req.notice.id && r.optNoticeType == OptNoticeType.Chat)
				.FirstOrDefaultAsync();
			if (rec == null) return BadRequest();


			rec.Push(req.notice.desc);
			await Common1DB.SaveChangesAsync();

			return Ok(new EditChat.Response
			{
				editedNotice = (ChatNotice)rec,
			});
		}

		public async Task<IActionResult> GetChatList([FromBody]GetChatList.Request req)
		{
			var res = new GetChatList.Response
			{
				notices = new List<ChatNotice>(),
			};

			var list = await Common1DB.OpsNotices
				.Where(r=>r.optNoticeType == OptNoticeType.Chat)
				.ToListAsync();
			list.ForEach(rec =>
			{
				res.notices.Add((ChatNotice)rec);
			});

			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> AddPopup([FromBody]AddPopup.Request req)
		{
			var newRec = new OpsNotice();
			newRec.Push(req.desc);

			await Common1DB.OpsNotices.AddAsync(newRec);
			await Common1DB.SaveChangesAsync();

			return Ok(new AddPopup.Response
			{
				addedNotice = (PopupNotice)newRec,
			});
		}

		public async Task<IActionResult> EditPopup([FromBody]EditPopup.Request req)
		{
			var rec = await Common1DB.OpsNotices
				.Where(r => r.Id == req.notice.id && r.optNoticeType == OptNoticeType.Popup)
				.FirstOrDefaultAsync();
			if (rec == null) return BadRequest();


			rec.Push(req.notice.desc);
			await Common1DB.SaveChangesAsync();

			return Ok(new EditPopup.Response
			{
				editedNotice = (PopupNotice)rec,
			});
		}

		public async Task<IActionResult> GetPopupList([FromBody]GetPopupList.Request req)
		{
			var res = new GetPopupList.Response
			{
				notices = new List<PopupNotice>(),
			};

			var list = await Common1DB.OpsNotices
				.Where(r => r.optNoticeType == OptNoticeType.Popup)
				.ToListAsync();
			list.ForEach(rec =>
			{
				res.notices.Add((PopupNotice)rec);
			});

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> AddTopics([FromBody]AddTopics.Request req)
		{
			var newRec = new OpsNotice();
			newRec.Push(req.desc);

			await Common1DB.OpsNotices.AddAsync(newRec);
			await Common1DB.SaveChangesAsync();

			return Ok(new AddTopics.Response
			{
				addedNotice = (TopicsNotice)newRec,
			});
		}

		public async Task<IActionResult> EditTopics([FromBody]EditTopics.Request req)
		{
			var rec = await Common1DB.OpsNotices
				.Where(r => r.Id == req.notice.id && r.optNoticeType == OptNoticeType.Topics)
				.FirstOrDefaultAsync();
			if (rec == null) return BadRequest();


			rec.Push(req.notice.desc);
			await Common1DB.SaveChangesAsync();

			return Ok(new EditTopics.Response
			{
				editedNotice = (TopicsNotice)rec,
			});
		}

		public async Task<IActionResult> GetTopicsList([FromBody]GetTopicsList.Request req)
		{
			var res = new GetTopicsList.Response
			{
				notices = new List<TopicsNotice>(),
			};

			var list = await Common1DB.OpsNotices
				.Where(r => r.optNoticeType == OptNoticeType.Topics)
				.ToListAsync();
			list.ForEach(rec =>
			{
				res.notices.Add((TopicsNotice)rec);
			});

			return Ok(res);
		}
	}
}
