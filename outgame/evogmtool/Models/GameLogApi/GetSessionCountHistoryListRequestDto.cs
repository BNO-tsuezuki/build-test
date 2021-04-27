using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evogmtool.Models.GameLogApi
{
    public class GetSessionCountHistoryListRequestDto
    {
        [BindRequired]
        public DateTime From { get; set; }

        [BindRequired]
        public DateTime To { get; set; }

        [BindRequired]
        public string AreaName { get; set; }
    }
}
