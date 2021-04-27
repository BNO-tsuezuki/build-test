using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace evolib
{
	public static class VivoxInfo
    {
        public static string issuer { get; private set; }
        public static string endPoint { get; private set; }
        public static string domain { get; private set; }
        public static string key { get; private set; }


        public static void Initialize(IConfiguration configuration)
		{
            issuer = configuration["Vivox:issuer"];
            endPoint = configuration["Vivox:endPoint"];
            domain = configuration["Vivox:domain"];
            key = configuration["Vivox:key"];
        }
	}
}
