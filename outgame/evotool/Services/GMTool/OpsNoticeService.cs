using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evolib;
using evolib.Databases.common1;
using evotool.Models;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;
using evotool.ProtocolModels.OpsNotice;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.GMTool
{
    public interface IOpsNoticeService
    {
        Task<(IList<OpsNotice>, int)> GetChatList(GetChatListRequest request);
        Task<OpsNotice> GetChat(long opsNoticeId);
        Task<OpsNotice> RegisterChat(ChatDesc chatDesc);
        Task<OpsNotice> UpdateChat(long opsNoticeId, ChatDesc chatDesc);
        Task DeleteChat(long opsNoticeId);

        Task<(IList<OpsNotice>, int)> GetPopupList(GetPopupListRequest request);
        Task<OpsNotice> GetPopup(long opsNoticeId);
        Task<OpsNotice> RegisterPopup(PopupDesc popupDesc);
        Task<OpsNotice> UpdatePopup(long opsNoticeId, PopupDesc popupDesc);
        Task DeletePopup(long opsNoticeId);
    }

    public class OpsNoticeService : BaseService, IOpsNoticeService
    {
        public OpsNoticeService(IServicePack servicePack) : base(servicePack)
        { }

        public async Task<(IList<OpsNotice>, int)> GetChatList(GetChatListRequest request)
        {
            var query = Common1DB.OpsNotices.Where(r => r.optNoticeType == OptNoticeType.Chat);

            if (request.From.HasValue) query = query.Where(x => x.endDate >= request.From.Value);

            if (request.To.HasValue) query = query.Where(x => x.beginDate <= request.To.Value);

            if (request.Target.HasValue) query = query.Where(x => x.target == request.Target.Value);

            var list = await query.OrderByDescending(x => x.Id)
                                  .Skip(request.CountPerPage * (request.PageNumber - 1))
                                  .Take(request.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return (list, count);
        }

        public async Task<OpsNotice> GetChat(long opsNoticeId)
        {
            var opsNotice = await Common1DB.OpsNotices
                                           .SingleOrDefaultAsync(x => x.Id == opsNoticeId &&
                                                                      x.optNoticeType == OptNoticeType.Chat);

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            return opsNotice;
        }

        public async Task<OpsNotice> RegisterChat(ChatDesc chatDesc)
        {
            var opsNotice = new OpsNotice();
            opsNotice.Push(chatDesc);

            await Common1DB.OpsNotices.AddAsync(opsNotice);

            await Common1DB.SaveChangesAsync();

            return opsNotice;
        }

        public async Task<OpsNotice> UpdateChat(long opsNoticeId, ChatDesc chatDesc)
        {
            var opsNotice = await Common1DB.OpsNotices
                .Where(r => r.Id == opsNoticeId && r.optNoticeType == OptNoticeType.Chat)
                .FirstOrDefaultAsync();

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            var isEdited = opsNotice.Push(chatDesc);

            if (opsNotice.release && chatDesc.release.Value && isEdited)
            {
                // todo: error message
                throw new BadRequestException("can not edit released notice");
            }

            await Common1DB.SaveChangesAsync();

            return opsNotice;
        }

        public async Task DeleteChat(long opsNoticeId)
        {
            var opsNotice = await Common1DB.OpsNotices
                .Where(r => r.Id == opsNoticeId && r.optNoticeType == OptNoticeType.Chat)
                .FirstOrDefaultAsync();

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            Common1DB.OpsNotices.Remove(opsNotice);

            await Common1DB.SaveChangesAsync();

            return;
        }

        public async Task<(IList<OpsNotice>, int)> GetPopupList(GetPopupListRequest request)
        {
            var query = Common1DB.OpsNotices.Where(r => r.optNoticeType == OptNoticeType.Popup);

            if (request.From.HasValue) query = query.Where(x => x.endDate >= request.From.Value);

            if (request.To.HasValue) query = query.Where(x => x.beginDate <= request.To.Value);

            if (request.Target.HasValue) query = query.Where(x => x.target == request.Target.Value);

            var list = await query.OrderByDescending(x => x.Id)
                                  .Skip(request.CountPerPage * (request.PageNumber - 1))
                                  .Take(request.CountPerPage)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return (list, count);
        }

        public async Task<OpsNotice> GetPopup(long opsNoticeId)
        {
            var opsNotice = await Common1DB.OpsNotices
                                           .SingleOrDefaultAsync(x => x.Id == opsNoticeId &&
                                                                      x.optNoticeType == OptNoticeType.Popup);

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            return opsNotice;
        }

        public async Task<OpsNotice> RegisterPopup(PopupDesc popupDesc)
        {
            var opsNotice = new OpsNotice();
            opsNotice.Push(popupDesc);

            await Common1DB.OpsNotices.AddAsync(opsNotice);

            await Common1DB.SaveChangesAsync();

            return opsNotice;
        }

        public async Task<OpsNotice> UpdatePopup(long opsNoticeId, PopupDesc popupDesc)
        {
            var opsNotice = await Common1DB.OpsNotices
                .Where(r => r.Id == opsNoticeId && r.optNoticeType == OptNoticeType.Popup)
                .FirstOrDefaultAsync();

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            var isEdited = opsNotice.Push(popupDesc);

            if (opsNotice.release && popupDesc.release.Value && isEdited)
            {
                // todo: error message
                throw new BadRequestException("can not edit released notice");
            }

            await Common1DB.SaveChangesAsync();

            return opsNotice;
        }

        public async Task DeletePopup(long opsNoticeId)
        {
            var opsNotice = await Common1DB.OpsNotices
                .Where(r => r.Id == opsNoticeId && r.optNoticeType == OptNoticeType.Popup)
                .FirstOrDefaultAsync();

            if (opsNotice == null)
            {
                // todo: error message
                throw new NotFoundException("not found");
            }

            Common1DB.OpsNotices.Remove(opsNotice);

            await Common1DB.SaveChangesAsync();

            return;
        }
    }
}
