using System.Collections.Generic;
using AutoMapper;
using evolib.Databases.common1;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;
using evotool.ProtocolModels.OpsNotice;

namespace evotool.AutoMapperProfiles
{
    public class OpsNoticeProfile : Profile
    {
        public OpsNoticeProfile()
        {
            CreateMap<(IList<OpsNotice>, int), GetChatListResponse>()
                .ForMember(d => d.List, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.TotalCount, opt => opt.MapFrom(s => s.Item2));
            CreateMap<OpsNotice, GetChatListResponse.Chat>()
                .ForMember(d => d.repeatedIntervalMinutes, opt => opt.MapFrom(s => s.repeateIntervalMinutes));

            CreateMap<OpsNotice, GetChatResponse>()
                .ForMember(d => d.repeatedIntervalMinutes, opt => opt.MapFrom(s => s.repeateIntervalMinutes));

            CreateMap<PostChatRequest, ChatDesc>();
            CreateMap<OpsNotice, PostChatResponse>()
                .ForMember(d => d.repeatedIntervalMinutes, opt => opt.MapFrom(s => s.repeateIntervalMinutes));

            CreateMap<PutChatRequest, ChatDesc>();
            CreateMap<OpsNotice, PutChatResponse>()
                .ForMember(d => d.repeatedIntervalMinutes, opt => opt.MapFrom(s => s.repeateIntervalMinutes));

            CreateMap<(IList<OpsNotice>, int), GetPopupListResponse>()
                .ForMember(d => d.List, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.TotalCount, opt => opt.MapFrom(s => s.Item2));
            CreateMap<OpsNotice, GetPopupListResponse.Popup>();

            CreateMap<OpsNotice, GetPopupResponse>();

            CreateMap<PostPopupRequest, PopupDesc>();
            CreateMap<OpsNotice, PostPopupResponse>();

            CreateMap<PutPopupRequest, PopupDesc>();
            CreateMap<OpsNotice, PutPopupResponse>();
        }
    }
}
