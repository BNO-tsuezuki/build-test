namespace MasterDataProcess.Models
{
    public class FileSettings
    {
        public string S3BucketName { get; set; }
        public string S3ObjectKeyMasterData { get; set; }
        public string S3ObjectKeyTranslationData { get; set; }
    }
}
