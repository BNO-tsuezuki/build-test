using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evogmtool.Models.GameLogApi
{
    public class GameLogRequestBaseDto
    {
        [BindRequired]
        public int CountPerPage { get; set; }

        [BindRequired]
        public int PageNumber { get; set; }

        [BindRequired]
        public DateTime From { get; set; }

        [BindRequired]
        public DateTime To { get; set; }
    }
}
