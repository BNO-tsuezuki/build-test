using System;

namespace evogmtool.Models.LogApi
{
    public class GetOperationLogResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Account { get; set; }
        public short StatusCode { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string Exception { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
