using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LogProcess.Databases.EvoGameLog;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LogProcess
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

        // todo: https://docs.aws.amazon.com/ja_jp/lambda/latest/dg/gettingstarted-limits.html
        // todo: https://docs.aws.amazon.com/ja_jp/lambda/latest/dg/best-practices.html

        // todo: tuning
        // todo: lambda instanceのmax memory上げれば劇的に速くなる→メモリだけじゃなくてCPUとかも連動して上がる
        // todo: cold start に時間がかかるのは lambda native である程度解消できるっぽい
        // todo: パッケージの依存関係？ 消しても動くのあったら消す
        // todo: 必要なものだけリリースする

        // todo: 同時実行数の制御 1つだけ？ 複数許可する？ rds proxy次第か？ S3のtriggerあふれたらどうなる？ 即エラー？

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            // todo: dbのproxy使った場合ってtranとかその失敗とかどうなる？
            // todo: DB接続のoverhead回避できるか？ aurora serverless？ の Data API Endpointとか検討してみる？ そもそもrds proxyってconnectionあるの？

            // todo: s3に定期的にファイルアップロードして実行時間を計測 10分以上あけるとインスタンス落ちる？

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

        private async Task<string> ProcessAsync(string s3BucketName, string s3ObjectKey)
        {
            var apiServerLogKeyword = Environment.GetEnvironmentVariable("apiServerLogKeyword");
            var dedicatedServerLogKeyword = Environment.GetEnvironmentVariable("dedicatedServerLogKeyword");

            if (!s3ObjectKey.ToLower().Contains(apiServerLogKeyword) &&
                !s3ObjectKey.ToLower().Contains(dedicatedServerLogKeyword))
            {
                return string.Empty;
            }

            var readCount = 0;
            var processCount = 0;
            var insertCount = 0;

            var sw = Stopwatch.StartNew();

            LinqToDBForEFTools.Initialize();

            var optBuilder = new LinqToDbConnectionOptionsBuilder();
            optBuilder.UseMySql(GetConnectionString());

            using (var dataConnection = new DataConnection(optBuilder.Build()))
            {
                // todo: 1つのeventに2つ以上のファイルが含まれることがないか確認する
                var request = new GetObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = s3ObjectKey,
                };

                using (var getResponse = await S3Client.GetObjectAsync(request))
                using (var streamReader = new StreamReader(getResponse.ResponseStream))
                using (var tran = dataConnection.BeginTransaction())
                {
                    try
                    {
                        if (s3ObjectKey.ToLower().Contains(apiServerLogKeyword))
                        {
                            (readCount, processCount, insertCount) = await new ApiServerLogProcessor(dataConnection).ProcessAsync(streamReader);
                        }
                        else if (s3ObjectKey.ToLower().Contains(dedicatedServerLogKeyword))
                        {
                            (readCount, processCount, insertCount) = await new DedicatedServerLogProcessor(dataConnection).ProcessAsync(streamReader);
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                return $"{sw.Elapsed.TotalMilliseconds:N0} msec. read: {readCount:N0} lines. process: {processCount:N0} lines. insert: {insertCount:N0} rows.";
            }
        }

        private string GetConnectionString()
        {
            var dbEndpoint = Environment.GetEnvironmentVariable("rdsproxy_endpoint");
            var dbName = Environment.GetEnvironmentVariable("dbname");
            var dbUser = Environment.GetEnvironmentVariable("dbuser");
            var dbPassword = Environment.GetEnvironmentVariable("dbpassword");
            return $"server={dbEndpoint}; database={dbName}; SslMode=none; user={dbUser}; password={dbPassword};";
        }
    }
}
