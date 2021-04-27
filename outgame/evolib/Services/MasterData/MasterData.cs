using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace evolib.Services.MasterData
{
	public interface IMasterData
	{
		int[] Version { get; }
		string VersionStr { get; }
		string DownloadPath { get; }
		string SecretePath { get; }
		List<string> AllMobileSuitIds { get; }
		IMobileSuit GetMobileSuit(string mobileSuitId);
		List<string> AllVoicePackItemIds { get; }
		IVoicePack GetVoicePack(string voicePackItemId);
		List<string> AllItemIds { get; }
		IItem GetItemFromItemId(string itemId);
		bool CheckDefaultOwnedItem(string itemId);
		List<string> DefaultOwnedItems { get; }
		string DefaultPlayerIcon { get;  }
		bool CheckNgWords(string str);
		ICareerRecord GetCareerRecord(string recordItemId);
		List<ICareerRecord> AllCareerRecords { get; }
		IBattleRatingTier GetBattleRatingTier(string id);
		PlayerInformation.BattleRatingTierType GetTierType(float battleRating, int battleNum);
        // BattlePass
        ISeason GetSeason(int seasonNo);
        List<ISeason> AllSeasons { get; }
        ISeason GetCurrentSeason();
        IBattlePass GetBattlePass(int passId);
        List<IBattlePass> AllBattlePasses { get; }
		List<IBattlePass> GetEnableBattlePass();
		IBattlePass GetSeasonPass(int seasonNo);
        IBattlePass GetPlayerPass();
        PassNeedExp GetPassExp(string tableKey);
        Dictionary<int, PassNeedExp.ExpInfo> GetExpSetting(string tableKey);
        bool CheckExpSetting(string tableKey);
        PassReward GetPassReward(string tableKey);
        bool CheckPassRewardSetting(string tableKey);
		InitialPlayerLevel InitialPlayerLevel { get; }
		// Achievement
		IReadOnlyList<IAchievement> AllAchievement { get; }
		IAchievement GetAchievement(string achievementId);
        // PresentBox
        IReadOnlyList<IPresentBox> AllPresentBox { get; }
        IPresentBox GetPresentBox(evolib.PresentBox.Type type);
        // SupplyPod
        List<string> AllSupplyPodIds { get; }
        ISupplyPod GetSupplyPod(string supplyPodId);
        List<string> AllSupplyPodLineupIds { get; }
        ISupplyPodLineup GetSupplyPodLineup(string supplyPodId);
        //Assets
        IAssetsInfo GetAssetsInfo(string assetsId);
        IAssetsInfo GetAssetsInfoByType(string type);
        // MaterialConversion
        IMaterialConversion GetMaterialConversion(evolib.Item.RankType rankType);
        // Challenge
        IReadOnlyList<IChallenge> AllChallenge { get; }
        IChallenge GetChallenge(string challengeId);
        IChallengeExpiration GetChallengeExpiration(evolib.Challenge.Type challengeType);
        IReadOnlyList<IBeginnerChallengeSheet> AllBeginnerChallengeSheet { get; }
        IBeginnerChallengeSheet GetBeginnerChallengeSheet(string sheetId);
    }

	public class MasterData : IMasterData
	{
		public int[] Version { get; set; }
		public string VersionStr { get { return $"{Version[0]}-{Version[1]}-{Version[2]}"; } }

		public string DownloadPath { get; set; }
		public string SecretePath { get; set; }

		public List<string> AllMobileSuitIds
		{
			get { return MobileSuits.Keys.ToList(); }
		}
		public IMobileSuit GetMobileSuit(string mobileSuitId)
		{
			return (MobileSuits.ContainsKey(mobileSuitId))? (MobileSuits[mobileSuitId]) : (null);
		}


		public List<string> AllVoicePackItemIds
		{
			get { return VoicePacks.Keys.ToList(); }
		}
		public IVoicePack GetVoicePack(string voicePackItemId)
		{
			return (VoicePacks.ContainsKey(voicePackItemId)) ? (VoicePacks[voicePackItemId]) : (null);
		}

		public List<string> AllItemIds
		{
			get { return ItemItemIdMap.Keys.ToList(); }
		}
		public IItem GetItemFromItemId(string itemId)
		{
			return (ItemItemIdMap.ContainsKey(itemId)) ? (ItemItemIdMap[itemId]) : (null);
		}

		static string AssetDataPath(string assetDataTablePath, string assetId)
		{
			return assetDataTablePath + "/" + assetId;
		}
		public IItem GetItemFromAssetDataPath(string assetDataTablePath, string assetId)
		{
			var key = AssetDataPath(assetDataTablePath, assetId);
			return (ItemAssetDataPathMap.ContainsKey(key)) ? (ItemAssetDataPathMap[key]) : (null);
		}

		public bool CheckDefaultOwnedItem(string itemId)
		{
			return _DefaultOwnedItems.ContainsKey(itemId);
		}

		public List<string> DefaultOwnedItems
		{
			get
			{
				return _DefaultOwnedItems.Keys.ToList();
			}
		}

		public string DefaultPlayerIcon { get; private set; }

		public bool CheckNgWords(string str)
		{
			if (false == Regex.IsMatch(str, _AllBlackWords) )
			{
				return false;
			}

			var blackMatches = Regex.Matches(str, _AllBlackWords);
			foreach( Match black in blackMatches)
			{
				var contained = false;

				var whiteMatches = Regex.Matches(str, _AllWhiteWords);
				foreach ( Match white in whiteMatches)
				{
					if (white.Index <= black.Index &&
						(black.Index + black.Length) <= (white.Index + white.Length))
					{
						contained = true;
						break;
					}
				}

				if (!contained)
				{
					return true;
				}
			}

			return false;
		}

		//----
		Dictionary<string, MobileSuit> MobileSuits = new Dictionary<string, MobileSuit>();
		public void AddMobileSuit( MobileSuit ms)
		{
			MobileSuits[ms.mobileSuitId] = ms;
		}

		Dictionary<string, VoicePack> VoicePacks = new Dictionary<string, VoicePack>();
		public void AddVoicePack( VoicePack vp )
		{
			VoicePacks[vp.itemId] = vp;
		}

		Dictionary<string, Item> ItemItemIdMap = new Dictionary<string, Item>();
		Dictionary<string, Item> ItemAssetDataPathMap = new Dictionary<string, Item>();
		Dictionary<evolib.Item.Type, List<IItem>> ItemsItemTypeMap;
		Dictionary<string, int> _DefaultOwnedItems = new Dictionary<string, int>();
		public void AddItem(Item item, string assetDataTablePath, string assetId)
		{
			ItemItemIdMap[item.itemId] = item;

			var key = AssetDataPath(assetDataTablePath, assetId);
			ItemAssetDataPathMap[key] = item;

			if (ItemsItemTypeMap == null)
			{
				ItemsItemTypeMap = new Dictionary<evolib.Item.Type, List<IItem>>();
				foreach (evolib.Item.Type itemType in Enum.GetValues(typeof(evolib.Item.Type)))
				{
					ItemsItemTypeMap[itemType] = new List<IItem>();
				}
			}
			ItemsItemTypeMap[item.itemType].Add(item);

			if( item.itemType==evolib.Item.Type.PlayerIcon && item.isDefaultSetting )
			{
				DefaultPlayerIcon = item.itemId;
			}
		}

		public void AddDefaultOwnedItem(string itemId)
		{
			_DefaultOwnedItems[itemId] = 1234;
		}

		string _AllBlackWords = "";
		string _AllWhiteWords = "";
		public void AddNgWord(string regex, bool black )
		{
			if (black)
			{
				if (!string.IsNullOrEmpty(_AllBlackWords))
				{
					_AllBlackWords += "|";
				}

				_AllBlackWords += regex;
			}
			else
			{
				if (!string.IsNullOrEmpty(_AllWhiteWords))
				{
					_AllWhiteWords += "|";
				}

				_AllWhiteWords += regex;
			}
		}

		public ICareerRecord GetCareerRecord(string recordItemId)
		{
			return (CareerRecords.ContainsKey(recordItemId)) ? (CareerRecords[recordItemId]) : (null);
		}
		public List<ICareerRecord> AllCareerRecords
		{
			get { return CareerRecords.Values.ToList<ICareerRecord>(); }
		}

		Dictionary<string, CareerRecord> CareerRecords = new Dictionary<string, CareerRecord>();
		public void AddCareerRecord(CareerRecord careerRecord)
		{
			CareerRecords[careerRecord.recordItemId] = careerRecord;
		}

		public IBattleRatingTier GetBattleRatingTier(string id)
		{
			return (BattleRatingTiers.ContainsKey(id)) ? (BattleRatingTiers[id]) : (null);
		}

		Dictionary<string, BattleRatingTier> BattleRatingTiers = new Dictionary<string, BattleRatingTier>();
		public void AddBattleRatingTier(BattleRatingTier battleRatingTier)
		{
			BattleRatingTiers[battleRatingTier.Id] = battleRatingTier;
		}
		public PlayerInformation.BattleRatingTierType GetTierType(float battleRating, int battleNum)
		{
			var tierType = PlayerInformation.BattleRatingTierType.Unrank;

			foreach (var battleRatingTier in BattleRatingTiers.Values)
			{
				float value = (battleRatingTier.isUnrank) ? battleNum : battleRating;

				// 上限指定が無い場合
				if (battleRatingTier.endRange <= -1 &&
					battleRatingTier.startRange <= value)
				{
					tierType = battleRatingTier.tierType;
					break;
				}
				// 開始と終了の範囲が指定されている場合
				else if (battleRatingTier.startRange <= value &&
					value <= battleRatingTier.endRange)
				{
					tierType = battleRatingTier.tierType;
					break;
				}
			}

			return tierType;
		}

        // -- BattlePass --
        // Season
        Dictionary<int, Season> Seasons = new Dictionary<int, Season>();
        public void AddSeasonRow(Season rowData)
        {
            Seasons[rowData.seasonNo] = rowData;
        }
        public ISeason GetSeason(int seasonNo)
        {
            return (Seasons.ContainsKey(seasonNo)) ? (Seasons[seasonNo]) : (null);
        }
        public List<ISeason> AllSeasons
        {
            get { return Seasons.Values.ToList<ISeason>(); }
        }
        public ISeason GetCurrentSeason()
        {
            var now = DateTime.UtcNow;
            foreach (var kvp in Seasons)
            {
                if (kvp.Value.startDate <= now && now < kvp.Value.endDate)
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        // Pass
        Dictionary<int, BattlePass> BattlePasses = new Dictionary<int, BattlePass>();
        public void AddBattlePassRow(BattlePass rowData)
        {
            BattlePasses[rowData.id] = rowData;
        }
        public IBattlePass GetBattlePass(int passId)
        {
            return (BattlePasses.ContainsKey(passId)) ? (BattlePasses[passId]) : (null);
        }
        public List<IBattlePass> AllBattlePasses
        {
            get { return BattlePasses.Values.ToList<IBattlePass>(); }
        }
        public IBattlePass GetSeasonPass(int seasonNo)
        {
            foreach (var kvp in BattlePasses)
            {
                if (kvp.Value.type == evolib.BattlePass.PassType.SeasonPass && kvp.Value.seasonNo == seasonNo)
                {
                    return kvp.Value;
                }
            }
            return null;
        }
        public IBattlePass GetPlayerPass()
        {
            foreach (var kvp in BattlePasses)
            {
                if (kvp.Value.type == evolib.BattlePass.PassType.PlayerLevel)
                {
                    return kvp.Value;
                }
            }
            return null;
        }
		// enable pass
		List<IBattlePass> EnableBattlePass = new List<IBattlePass>();
		public void CreateEnableBattlePass()
		{
			var season = GetCurrentSeason();
			var masterList = AllBattlePasses;
			foreach (var row in masterList)
			{
				if (row.type == evolib.BattlePass.PassType.PlayerLevel)
				{
					EnableBattlePass.Add(row);
				}
				else if (row.type == evolib.BattlePass.PassType.SeasonPass && (season != null && row.seasonNo == season.seasonNo))
				{
					EnableBattlePass.Add(row);
				}
				else
				{
					if (row.startDate <= DateTime.UtcNow && DateTime.UtcNow < row.endDate)
					{
						EnableBattlePass.Add(row);
					}
				}
			}
		}
		public List<IBattlePass> GetEnableBattlePass()
		{
			return EnableBattlePass;
		}

		// PassNeedExp
		Dictionary<string, PassNeedExp> PassNeedExps = new Dictionary<string, PassNeedExp>();
        public void AddPassNeedExpData(string tableKey, PassNeedExp tableData)
        {
            PassNeedExps[tableKey] = tableData;
        }
        public PassNeedExp GetPassExp(string tableKey)
        {
            return PassNeedExps[tableKey];
        }
        public Dictionary<int, PassNeedExp.ExpInfo> GetExpSetting(string tableKey)
        {
            return PassNeedExps[tableKey].GetSettings();
        }
        public bool CheckExpSetting(string tableKey)
        {
            return PassNeedExps.ContainsKey(tableKey);
        }

        // PassReward
        Dictionary<string, PassReward> PassRewards = new Dictionary<string, PassReward>();
        public void AddPassRewardData(string tableKey, PassReward tableData)
        {
            PassRewards[tableKey] = tableData;
        }
        public PassReward GetPassReward(string tableKey)
        {
            return PassRewards[tableKey];
        }
        public bool CheckPassRewardSetting(string tableKey)
        {
            return PassRewards.ContainsKey(tableKey);
        }

		InitialPlayerLevel _InitialPlayerLevel = new InitialPlayerLevel();
		public InitialPlayerLevel InitialPlayerLevel
		{
			get { return _InitialPlayerLevel; }
		}

        // Reward
		Dictionary<string, Reward> Rewards = new Dictionary<string, Reward>();
        public void AddReward(Reward rwd)
        {
            Rewards[rwd.rewardId] = rwd;
        }
        public Reward GetReward(string rewardId)
        {
			return (Rewards.ContainsKey(rewardId)) ? (Rewards[rewardId]) : (null);
        }

		// Achievement
		Dictionary<string, IAchievement> _AllAchievement = new Dictionary<string, IAchievement>();
		public void AddAchievement(IAchievement achi)
		{
			_AllAchievement[achi.achievementId] = achi;
		}
		public IReadOnlyList<IAchievement> AllAchievement
		{
			get { return _AllAchievement.Values.ToList<IAchievement>(); }
		}
		public IAchievement GetAchievement(string achievementId)
		{
			return (_AllAchievement.ContainsKey(achievementId)) ? (_AllAchievement[achievementId]) : (null);
		}

        // PresentBox
        Dictionary<evolib.PresentBox.Type, IPresentBox> _AllPresentBox = new Dictionary<evolib.PresentBox.Type, IPresentBox>();
        public void AddPresentBox(IPresentBox pb)
        {
            _AllPresentBox[pb.type] = pb;
        }
        public IReadOnlyList<IPresentBox> AllPresentBox
        {
            get { return _AllPresentBox.Values.ToList<IPresentBox>(); }
        }
        public IPresentBox GetPresentBox(evolib.PresentBox.Type type)
        {
            return (_AllPresentBox.ContainsKey(type)) ? (_AllPresentBox[type]) : (null);
        }

        // SupplyPod
        Dictionary<string, SupplyPod> SupplyPods = new Dictionary<string, SupplyPod>();
        public void AddSupplyPod(SupplyPod sp)
        {
            SupplyPods[sp.supplyPodId] = sp;
        }
        public List<string> AllSupplyPodIds
        {
            get { return SupplyPods.Keys.ToList(); }
        }
        public ISupplyPod GetSupplyPod(string supplyPodId)
        {
            return (SupplyPods.ContainsKey(supplyPodId)) ? (SupplyPods[supplyPodId]) : (null);
        }

        Dictionary<string, SupplyPodLineup> SupplyPodLineups = new Dictionary<string, SupplyPodLineup>();
        public void AddSupplyPodLineup(SupplyPodLineup spl)
        {
            SupplyPodLineups[spl.supplyPodId] = spl;
        }
        public List<string> AllSupplyPodLineupIds
        {
            get { return SupplyPodLineups.Keys.ToList(); }
        }
        public ISupplyPodLineup GetSupplyPodLineup(string supplyPodId)
        {
            return (SupplyPodLineups.ContainsKey(supplyPodId)) ? (SupplyPodLineups[supplyPodId]) : (null);
        }

		// Assets
		Dictionary<string, AssetsInfo> _AssetsInfoMap = new Dictionary<string, AssetsInfo>();
		public IAssetsInfo GetAssetsInfo(string assetsId)
		{
			if (!_AssetsInfoMap.ContainsKey(assetsId)) return null;
			return _AssetsInfoMap[assetsId];
		}
        public IAssetsInfo GetAssetsInfoByType(string type)
        {
            foreach (KeyValuePair<string, AssetsInfo> _AssetsInfo in _AssetsInfoMap)
            {
                if (_AssetsInfo.Value.type == type)
                {
                    return _AssetsInfo.Value;
                }
            }
            return null;
        }
        public void AddAssetsInfo(AssetsInfo assets)
		{
			_AssetsInfoMap[assets.assetsId] = assets;
		}

        // MaterialConversion
        Dictionary<evolib.Item.RankType, MaterialConversion> MaterialConversions = new Dictionary<evolib.Item.RankType, MaterialConversion>();
        public void AddMaterialConversion(MaterialConversion mc)
        {
            MaterialConversions[mc.rankType] = mc;
        }
        public IMaterialConversion GetMaterialConversion(evolib.Item.RankType rankType)
        {
            return (MaterialConversions.ContainsKey(rankType)) ? (MaterialConversions[rankType]) : (null);
        }

        // Challenge
		Dictionary<string, Challenge> _ChallengeMap = new Dictionary<string, Challenge>();
		public void AddChallenge(Challenge ch)
		{
			_ChallengeMap[ch.challengeId] = ch;
		}
		public IReadOnlyList<IChallenge> AllChallenge
		{
			get { return _ChallengeMap.Values.ToList<IChallenge>(); }
		}
		public IChallenge GetChallenge(string challengeId)
		{
			return (_ChallengeMap.ContainsKey(challengeId)) ? (_ChallengeMap[challengeId]) : (null);
		}
		Dictionary<evolib.Challenge.Type, ChallengeExpiration> _ChallengeExpirationMap = new Dictionary<evolib.Challenge.Type, ChallengeExpiration>();
		public void AddChallengeExpiration(ChallengeExpiration expiration)
		{
			_ChallengeExpirationMap[expiration.type] = expiration;
		}
		public IChallengeExpiration GetChallengeExpiration(evolib.Challenge.Type challengeType)
		{
			return (_ChallengeExpirationMap.ContainsKey(challengeType)) ? (_ChallengeExpirationMap[challengeType]) : (null);
		}
		Dictionary<string, BeginnerChallengeSheet> _BeginnerChallengeSheetMap = new Dictionary<string, BeginnerChallengeSheet>();
		public void AddBeginnerChallengeSheet(BeginnerChallengeSheet sheet)
		{
			_BeginnerChallengeSheetMap[sheet.sheetId] = sheet;
		}
		public IReadOnlyList<IBeginnerChallengeSheet> AllBeginnerChallengeSheet
		{
			get { return _BeginnerChallengeSheetMap.Values.ToList<IBeginnerChallengeSheet>(); }
		}
		public IBeginnerChallengeSheet GetBeginnerChallengeSheet(string sheetId)
		{
			return (_BeginnerChallengeSheetMap.ContainsKey(sheetId)) ? (_BeginnerChallengeSheetMap[sheetId]) : (null);
		}
    }
}

