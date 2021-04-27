using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.OpsNotice;



namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class OpsNoticeController : BaseController
	{
		public OpsNoticeController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }


        [HttpPost]
		public IActionResult GetNotices([FromBody]GetNotices.Request req)
		{
            var res = new GetNotices.Response
            {
                chatNotices = new List<GetNotices.ChatNotice>(),
                popupNotices = new List<GetNotices.PopupNotice>(),
                topicsNotices = new List<GetNotices.TopicsNotice>(),
			};

			var notices = OpsNoticeManager.Notices;

			req.opsNoticeCodes.ForEach(code =>
			{
				if ( !notices.ContainsKey(code) )
				{
					return;
				}

				var rec = notices[code];

				if( rec.optNoticeType == OptNoticeType.Chat)
				{
					res.chatNotices.Add(
						rec.Pop(new GetNotices.ChatNotice
						{
							noticeCode = code,
						})
					);
				}
				if (rec.optNoticeType == OptNoticeType.Popup)
				{
					res.popupNotices.Add(
						rec.Pop(new GetNotices.PopupNotice
						{
							noticeCode = code,
						})
					);
				}
                if (rec.optNoticeType == OptNoticeType.Topics)
                {
                    res.topicsNotices.Add(
                        rec.Pop(new GetNotices.TopicsNotice
                        {
                            noticeCode = code,
                        })
                    );
                }
			});
			
			return Ok(res);
		}
	}
}
