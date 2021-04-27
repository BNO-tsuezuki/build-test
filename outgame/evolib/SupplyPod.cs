using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public static class SupplyPod
    {
        public enum PlayType
        {
            Single = 0,
            Package,
        }

        public enum Type
        {
            Normal = 0,
            Box,
        }

        public enum ConsumeType
        {
            EC = 0,
            CP,
            Ticket,
        }

        public enum LotteryResultCode
        {
            Success = 0,
            LineupError,
            LotteryError,
        }

        public enum GetRewardResultCode
        {
            Success = 0,
            Error,
        }

        public class LotteryInfo
        {
            public LotteryResultCode resultCode { get; set; }
            public string itemId { get; set; }
        }

        public class LotteryResult
        {
            public List<string> itemIds { get; set; }
        }

        public static int PackagePlayNum { get { return 11; } }

        public static int ECPlayPodNum { get { return 3; } }

        public static Item.RankType HighRankType { get { return Item.RankType.Bronze; } }
    }
}
