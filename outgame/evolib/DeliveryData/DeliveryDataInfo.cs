using Microsoft.Extensions.Configuration;

namespace evolib.DeliveryData
{
    public static class DeliveryDataInfo
	{
		public static string S3BucketName { get; private set; }
		public static string S3BucketRegion { get; private set; }
		public static string CfDomainName { get; private set; }

		public static void Initialize(IConfiguration configuration)
		{
			S3BucketName = configuration["DeliveryData:S3BucketName"];
			S3BucketRegion = configuration["DeliveryData:S3BucketRegion"];
			CfDomainName = configuration["DeliveryData:CfDomainName"];
		}
	}
}
