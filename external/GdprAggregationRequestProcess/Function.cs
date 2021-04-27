using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using LogProcess.Databases.EvoGameLog;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GdprAggregationRequestProcess
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
            var credentials = GetAWSCredentials();

            if (credentials == null)
            {
                S3Client = new AmazonS3Client();
            }
            else
            {
                var region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("s3_region"));
                S3Client = new AmazonS3Client(credentials, region);
            }
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        public Function(IAmazonS3 s3Client)
        {
            S3Client = s3Client;
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            // todo: ★ ログdbのデータ量次第ではメモリ結構必要になる？
            // todo: ★ SQSのqueue削除は成功を返すだけ？ 要確認
            // todo: ★ タイムアウト設定に注意

            if (sqsEvent.Records.Count != 1)
            {
                throw new Exception($"[ERROR] SQS event count error: {sqsEvent.Records.Count}");
            }

            var account = sqsEvent.Records.First().Body;

            LambdaLogger.Log($"[INFO ] Account: {account}");

            try
            {
                await ProcessAsync(account);
            }
            catch (Exception e)
            {
                LambdaLogger.Log($"[ERROR] {e.Message}\n{e.StackTrace}");
                throw;
            }
        }

        private async Task ProcessAsync(string account)
        {
            var sw = Stopwatch.StartNew();

            var jsonObject = await GetData(account);

            // todo: ★indent 要確認
            var jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            UploadJsonFileToS3(jsonString, account);

            LambdaLogger.Log($"[INFO ] {sw.Elapsed.TotalMilliseconds:N0} msec. length: {jsonString.Length:N0}.");
        }

        private async Task<object> GetData(string account)
        {
            var evotoolBaseUri = Environment.GetEnvironmentVariable("evotool_base_uri");
            var gameDataRepository = new GameDataRepository(evotoolBaseUri);

            var playerId = await gameDataRepository.GetPlayerId(account);

            if (playerId == null)
            {
                LambdaLogger.Log($"[ERROR] Account not found.");

                return new
                {
                    gameData = new { },
                    gameLog = new { },
                };
            }

            LambdaLogger.Log($"[INFO ] PlayerId: {playerId.Value}");

            var optBuilder = new DbContextOptionsBuilder<EvoGameLogDbContext>();
            optBuilder.UseMySql(GetGameLogDbConnectionString());

            using (var gameLogDbContext = new EvoGameLogDbContext(optBuilder.Options))
            {
                var gameLogRepository = new GameLogRepository(gameLogDbContext);

                Task<object> getGameDataTask = gameDataRepository.GetGameDataAsync(account);
                Task<object> getGameLogTask = gameLogRepository.GetGameLogAsync(playerId.Value);

                var gameData = await getGameDataTask;
                var gameLog = await getGameLogTask;

                return new
                {
                    gameData,
                    gameLog,
                };
            }
        }

        private void UploadJsonFileToS3(string json, string account)
        {
            var bucketName = Environment.GetEnvironmentVariable("s3_bucket_name");
            var gameServerInformation = Environment.GetEnvironmentVariable("game_server_information");
            var key = $"{account}_{gameServerInformation}_{DateTime.UtcNow.ToString("yyyy-MM-ddTHHmmssZ")}.json";

            var fileTransferUtility = new TransferUtility(S3Client);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                fileTransferUtility.Upload(ms, bucketName, key);
            }
        }

        private string GetGameLogDbConnectionString()
        {
            var dbEndpoint = Environment.GetEnvironmentVariable("gamelog_rdsproxy_endpoint");
            var dbName = Environment.GetEnvironmentVariable("gamelog_dbname");
            var dbUser = Environment.GetEnvironmentVariable("gamelog_dbuser");
            var dbPassword = Environment.GetEnvironmentVariable("gamelog_dbpassword");
            return $"server={dbEndpoint}; database={dbName}; SslMode=none; user={dbUser}; password={dbPassword};";
        }

        private AWSCredentials GetAWSCredentials()
        {
            var secretString = GetSecretString();

            if (string.IsNullOrWhiteSpace(secretString))
            {
                return null;
            }

            var secretJson = JObject.Parse(secretString);

            var accessKey = secretJson?["s3_access_key"]?.ToString();
            var secretKey = secretJson?["s3_secret_key"]?.ToString();

            if (accessKey == null || secretKey == null)
            {
                return null;
            }

            return new BasicAWSCredentials(accessKey, secretKey);
        }

        public string GetSecretString()
        {
            var region = Environment.GetEnvironmentVariable("secret_region");
            var secretName = Environment.GetEnvironmentVariable("secret_name");

            if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(secretName))
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

                var request = new GetSecretValueRequest();
                request.SecretId = secretName;
                request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

                var response = client.GetSecretValueAsync(request).Result;

                return response.SecretString;
            }
        }
    }
}
