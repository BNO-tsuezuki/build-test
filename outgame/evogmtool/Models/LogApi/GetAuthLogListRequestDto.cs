using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evogmtool.Models.LogApi
{
    public class GetAuthLogListRequestDto
    {
        [BindRequired]
        public DateTime From { get; set; }

        [BindRequired]
        public DateTime To { get; set; }

        public string Account { get; set; }

        public sbyte? Result { get; set; }

        public string IpAddress { get; set; }
    }
}
