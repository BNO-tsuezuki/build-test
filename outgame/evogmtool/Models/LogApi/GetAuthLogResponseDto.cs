using System;

namespace evogmtool.Models.LogApi
{
    public class GetAuthLogResponseDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public sbyte Result { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
