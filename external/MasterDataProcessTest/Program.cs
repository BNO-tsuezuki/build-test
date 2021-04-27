using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.TestUtilities;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace MasterDataProcessTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var awsProfile = Environment.GetEnvironmentVariable("TEST_AWS_PROFILE");
                var awsRegion = Environment.GetEnvironmentVariable("TEST_AWS_REGION");

                // https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
                // C:\users\{windows_user_name}\.aws\credentials
                var chain = new CredentialProfileStoreChain();
                AWSCredentials credentials;
                chain.TryGetAWSCredentials(awsProfile, out credentials);

                var region = RegionEndpoint.GetBySystemName(awsRegion);

                var s3Client = new AmazonS3Client(credentials, region);

                var lambdaFunction = new MasterDataProcess.Function(s3Client);

                var input = new MasterDataProcess.Models.FileSettings
                {
                    S3BucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME"),
                    S3ObjectKeyMasterData = Environment.GetEnvironmentVariable("S3_OBJECT_KEY_MASTER_DATA"),
                    S3ObjectKeyTranslationData = Environment.GetEnvironmentVariable("S3_OBJECT_KEY_TRANSLATION_DATA")
                };

                var context = new TestLambdaContext();

                await lambdaFunction.FunctionHandler(input, context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
