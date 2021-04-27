using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Google.Cloud.BigQuery.V2;
using MasterDataProcess.Models;
using MasterDataProcess.Models.Master;
using MasterDataProcess.Models.Translation;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MasterDataProcess
{
    public class Function
    {
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(FileSettings input, ILambdaContext context)
        {
            try
            {
                context.Logger.LogLine($"S3BucketName:{input.S3BucketName}, S3ObjectKeyMasterData:{input.S3ObjectKeyMasterData}, S3ObjectKeyTranslationData:{input.S3ObjectKeyTranslationData}");

                var translationDataJson = await GetTranslationDataJsonAsync(input);

                var masterData = await GetMasterDataAsync(input, translationDataJson);

                var resultMessage = await ProcessAsync(masterData);

                if (!string.IsNullOrWhiteSpace(resultMessage))
                {
                    context.Logger.LogLine(resultMessage);
                }

                return resultMessage;
            }
            catch (AmazonS3Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
            catch (Exception e)
            {
                context.Logger.LogLine($"Error");
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }

        private async Task<TranslationDataJson> GetTranslationDataJsonAsync(FileSettings settings)
        {
            var translationDataText = await GetS3FileTextAsync(settings.S3BucketName, settings.S3ObjectKeyTranslationData);
            var translationDataJson = JsonSerializer.Deserialize<TranslationDataJson>(translationDataText);

            return translationDataJson;
        }

        private async Task<MasterData> GetMasterDataAsync(FileSettings settings, TranslationDataJson translationDataJson)
        {
            var masterDataText = await GetS3FileTextAsync(settings.S3BucketName, settings.S3ObjectKeyMasterData);
            var masterDataJson = JsonSerializer.Deserialize<MasterDataJson>(masterDataText);

            return new MasterData(masterDataJson, translationDataJson);
        }

        private async Task<string> ProcessAsync(MasterData masterData)
        {
            var projectId = Environment.GetEnvironmentVariable("BQ_PROJECT_ID");
            var datasetId = Environment.GetEnvironmentVariable("BQ_DATASET_ID");

            var sw = Stopwatch.StartNew();

            // https://cloud.google.com/docs/authentication/getting-started
            // environment variable GOOGLE_APPLICATION_CREDENTIALS
            using (var client = await BigQueryClient.CreateAsync(projectId))
            {
                await new MasterDataProcessor(client, datasetId, masterData).ProcessAsync();
            }

            return $"[INFO ] {sw.Elapsed.TotalMilliseconds:N0} msec.";
        }

        private async Task<string> GetS3FileTextAsync(string s3BucketName, string s3ObjectKey)
        {
            var request = new GetObjectRequest
            {
                BucketName = s3BucketName,
                Key = s3ObjectKey,
            };

            using (var getResponse = await S3Client.GetObjectAsync(request))
            using (var streamReader = new StreamReader(getResponse.ResponseStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
