using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.TestUtilities;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Util;

namespace AnalysisLogProcessTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            var awsProfile = Environment.GetEnvironmentVariable("TEST_AWS_PROFILE");
            var awsRegion = Environment.GetEnvironmentVariable("TEST_AWS_REGION");
            var s3BucketName = Environment.GetEnvironmentVariable("TEST_S3_BUCKET_NAME");
            var s3ObjectKey = Environment.GetEnvironmentVariable("TEST_S3_OBJECT_KEY");

            // https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
            // C:\users\{windows_user_name}\.aws\credentials
            var chain = new CredentialProfileStoreChain();
            AWSCredentials credentials;
            chain.TryGetAWSCredentials(awsProfile, out credentials);

            var region = RegionEndpoint.GetBySystemName(awsRegion);

            var s3Client = new AmazonS3Client(credentials, region);

            try
            {
                var lambdaFunction = new AnalysisLogProcess.Function(s3Client);

                var s3Event = new S3Event
                {
                    Records = new System.Collections.Generic.List<S3EventNotification.S3EventNotificationRecord>
                    {
                        new S3EventNotification.S3EventNotificationRecord
                        {
                            S3 = new S3EventNotification.S3Entity
                            {
                                Bucket = new S3EventNotification.S3BucketEntity { Name = s3BucketName },
                                Object = new S3EventNotification.S3ObjectEntity { Key = s3ObjectKey },
                            }
                        }
                    }
                };

                var context = new TestLambdaContext();

                await lambdaFunction.FunctionHandler(s3Event, context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
