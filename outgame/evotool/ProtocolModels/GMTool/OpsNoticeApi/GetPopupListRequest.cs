using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evotool.ProtocolModels.GMTool.OpsNoticeApi
{
    public class GetPopupListRequest
    {
        [BindRequired]
        [Range(1, int.MaxValue)]
        public int CountPerPage { get; set; }

        [BindRequired]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        // todo: from < to
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public ulong? Target { get; set; }
    }
}
