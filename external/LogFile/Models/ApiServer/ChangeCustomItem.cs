namespace LogFile.Models.ApiServer
{
    public class ChangeCustomItem
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public string UnitId { get; set; }
        public int ItemType { get; set; }
        public string ItemId { get; set; }
    }
}
