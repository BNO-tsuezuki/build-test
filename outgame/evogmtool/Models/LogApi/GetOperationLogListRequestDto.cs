using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evogmtool.Models.LogApi
{
    public class GetOperationLogListRequestDto
    {
        [BindRequired]
        public DateTime From { get; set; }

        [BindRequired]
        public DateTime To { get; set; }

        public int? UserId { get; set; }

        public short? StatusCode { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public string QueryString { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string Exception { get; set; }

        public string IpAddress { get; set; }
    }
}
