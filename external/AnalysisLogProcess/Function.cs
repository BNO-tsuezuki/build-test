using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Google.Cloud.BigQuery.V2;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AnalysisLogProcess
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
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            // todo: 1つのeventに2つ以上のファイルが含まれることがないか確認する
            var s3Event = evnt.Records?[0].S3;

            if (s3Event == null)
            {
                return null;
            }

            context.Logger.LogLine($"BucketName: {s3Event.Bucket.Name}  ObjectKey: {s3Event.Object.Key} ");

            try
            {
                var resultMessage = await ProcessAsync(s3Event.Bucket.Name, s3Event.Object.Key);

                if (!string.IsNullOrWhiteSpace(resultMessage))
                {
                    context.Logger.LogLine(resultMessage);
                }

                return resultMessage;
            }
            catch (AmazonS3Exception e)
            {
                context.Logger.LogLine($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
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

        public async Task<string> ProcessAsync(string s3BucketName, string s3ObjectKey)
        {
            var apiServerLogKeyword = Environment.GetEnvironmentVariable("S3_API_SERVER_LOG_KEYWORD");
            var dedicatedServerLogKeyword = Environment.GetEnvironmentVariable("S3_DEDICATED_SERVER_LOG_KEYWORD");

            if (!s3ObjectKey.ToLower().Contains(apiServerLogKeyword) &&
                !s3ObjectKey.ToLower().Contains(dedicatedServerLogKeyword))
            {
                return string.Empty;
            }

            var readCount = 0;
            var processCount = 0;
            var insertCount = 0;

            var projectId = Environment.GetEnvironmentVariable("BQ_PROJECT_ID");
            var datasetId = Environment.GetEnvironmentVariable("BQ_DATASET_ID");

            var sw = Stopwatch.StartNew();

            // https://cloud.google.com/docs/authentication/getting-started
            // environment variable GOOGLE_APPLICATION_CREDENTIALS
            using (var client = await BigQueryClient.CreateAsync(projectId))
            {
                var request = new GetObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = s3ObjectKey,
                };

                using (var getResponse = await S3Client.GetObjectAsync(request))
                using (var streamReader = new StreamReader(getResponse.ResponseStream))
                {
                    try
                    {
                        if (s3ObjectKey.ToLower().Contains(apiServerLogKeyword))
                        {
                            (readCount, processCount, insertCount) = await new ApiServerLogProcessor(client, datasetId).ProcessAsync(streamReader);
                        }
                        else if (s3ObjectKey.ToLower().Contains(dedicatedServerLogKeyword))
                        {
                            (readCount, processCount, insertCount) = await new DedicatedServerLogProcessor(client, datasetId).ProcessAsync(streamReader);
                        }
                    }
                    catch
                    {
                        // todo: BigQueryにtransactionの概念はあるか？
                        // todo: ログ出力は呼び出し元に任せるから、transactionの概念がないならここでのtry-catchは不要
                        //context.Logger.LogLine("rollback");
                        throw;
                    }
                }
            }

            return $"{sw.Elapsed.TotalMilliseconds:N0} msec. read: {readCount:N0} lines. process: {processCount:N0} lines. insert: {insertCount:N0} rows.";
        }
    }
}
