using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evolib;
using evolib.Databases.personal;
using evolib.Services.MasterData;
using evotool.Models;
using evotool.ProtocolModels.GMTool.PlayerApi;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace evotool.Services.GMTool
{
    public interface IPlayerService
    {
        Task<GetPlayerSearchResponse> GetPlayerSearch(GetPlayerSearchRequest request);
        Task<GetPlayerResponse> GetPlayer(long playerId);
        Task<GetPlayerNameResponse> GetPlayerName(long playerId);
        Task<PutPlayerNameResponse> PutPlayerName(long playerId, PutPlayerNameRequest request);
        Task<GetPlayerAppOptionResponse> GetPlayerAppOption(long playerId, int category);
        Task<PutPlayerAppOptionResponse> PutPlayerAppOption(long playerId, int category, PutPlayerAppOptionRequest request);
        Task<GetPlayerMobileSuitOptionResponse> GetPlayerMobileSuitOption(long playerId, string mobileSuitId);
        Task<PutPlayerMobileSuitOptionResponse> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request);
        Task<GetPlayerItemResponse> GetPlayerItem(long playerId);
        Task<PutPlayerItemResponse> PutPlayerItem(long playerId, PutPlayerItemRequest request);
        Task<GetPlayerPassResponse> GetPlayerPass(long playerId);
        Task<PutPlayerPassResponse> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request);
        Task<GetPlayerExpResponse> GetPlayerExp(long playerId);
        Task<PutPlayerExpResponse> PutPlayerExp(long playerId, PutPlayerExpRequest request);
        Task<GetPlayerTutorialResponse> GetPlayerTutorial(long playerId);
        Task<PutPlayerTutorialResponse> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request);
        Task<PutPlayerTutorialResetResponse> PutPlayerTutorialReset(long playerId);
        Task<GetPlayerCareerRecordResponse> GetPlayerCareerRecord(long playerId, int seasonNo, string mobileSuitId);
        Task<PutPlayerCareerRecordResponse> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request);
    }

    public class PlayerService : BaseService, IPlayerService
    {
        private readonly ITranslationService _translationService;

        public PlayerService(IServicePack servicePack, ITranslationService translationService) : base(servicePack)
        {
            _translationService = translationService;
        }

        public async Task<GetPlayerSearchResponse> GetPlayerSearch(GetPlayerSearchRequest request)
        {
            var response = new GetPlayerSearchResponse();
            response.players = new List<GetPlayerSearchResponse.Player>();

            if (request.playerId.HasValue)
            {
                var playerBasicInformation = await PDBSM.PersonalDBContext(request.playerId.Value)
                                                        .PlayerBasicInformations
                                                        .FindAsync(request.playerId.Value);

                if (playerBasicInformation == null)
                {
                    return response;
                }

                var player = await Common1DB.PlayerIds.FindAsync(request.playerId.Value);

                if (player == null)
                {
                    return response;
                }

                response.players.Add(new GetPlayerSearchResponse.Player
                {
                    playerId = playerBasicInformation.playerId,
                    playerName = playerBasicInformation.playerName,
                    account = player.account,
                    accountType = player.accountType.ToString(),
                });
            }
            else if (!string.IsNullOrWhiteSpace(request.playerName))
            {
                var matched = await Common2DB.PlayerNames.FindAsync(request.playerName);

                if (matched != null)
                {
                    var account = await Common1DB.PlayerIds.FindAsync(matched.playerId);

                    if (account != null)
                    {
                        response.players.Add(new GetPlayerSearchResponse.Player
                        {
                            playerId = matched.playerId,
                            playerName = matched.playerName,
                            account = account.account,
                            accountType = account.accountType.ToString(),
                        });

                        return response;
                    }
                }

                var partiallyMatchedPlayerNames = await Common2DB.PlayerNames.Where(x => x.playerName.StartsWith(request.playerName))
                                                                             .Take(50)  // todo: 件数 要検討
                                                                             .ToListAsync();

                foreach (var matchedPlayerName in partiallyMatchedPlayerNames)
                {
                    var account = await Common1DB.PlayerIds.FindAsync(matchedPlayerName.playerId);

                    if (account == null)
                    {
                        continue;
                    }

                    response.players.Add(new GetPlayerSearchResponse.Player
                    {
                        playerId = matchedPlayerName.playerId,
                        playerName = matchedPlayerName.playerName,
                        account = account.account,
                        accountType = account.accountType.ToString(),
                    });
                }
            }
            else if (!string.IsNullOrWhiteSpace(request.account))
            {
                var matchedAccounts = await Common1DB.Accounts.Where(x => x.account == request.account).ToListAsync();

                foreach (var matchedAccount in matchedAccounts)
                {
                    var playerBasicInformation = await PDBSM.PersonalDBContext(matchedAccount.playerId)
                                                            .PlayerBasicInformations
                                                            .FindAsync(matchedAccount.playerId);

                    if (playerBasicInformation == null)
                    {
                        continue;
                    }

                    response.players.Add(new GetPlayerSearchResponse.Player
                    {
                        playerId = playerBasicInformation.playerId,
                        playerName = playerBasicInformation.playerName,
                        account = matchedAccount.account,
                        accountType = matchedAccount.type.ToString(),
                    });
                }
            }
            else
            {
                // todo: error message
                throw new BadRequestException(string.Empty);
            }

            return response;
        }

        public async Task<GetPlayerResponse> GetPlayer(long playerId)
        {
            var response = new GetPlayerResponse();

            var playerIdRecord = await Common1DB.PlayerIds.FindAsync(playerId);

            if (playerIdRecord == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var accountRecord = await Common1DB.Accounts.FindAsync(playerIdRecord.account, playerIdRecord.accountType);

            if (accountRecord == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var playerBasicInformation = await PDBSM.PersonalDBContext(playerId)
                                                    .PlayerBasicInformations
                                                    .FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            response.Player = new GetPlayerResponse._Player()
            {
                PlayerId = playerBasicInformation.playerId,
                PlayerName = playerBasicInformation.playerName,
                PlatformId = accountRecord.type.ToString(),
                Account = accountRecord.account,
                CountryCode = accountRecord.countryCode,
                CreatedDate = accountRecord.inserted,
                Discipline = accountRecord.banExpiration,
            };

            return response;
        }

        public async Task<GetPlayerNameResponse> GetPlayerName(long playerId)
        {
            var response = new GetPlayerNameResponse();

            var playerBasicInformation = await PDBSM.PersonalDBContext(playerId)
                                                    .PlayerBasicInformations
                                                    .FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            response.player = new GetPlayerNameResponse.Player()
            {
                playerName = playerBasicInformation.playerName,
            };

            return response;
        }

        public async Task<PutPlayerNameResponse> PutPlayerName(long playerId, PutPlayerNameRequest request)
        {
            var response = new PutPlayerNameResponse();

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            response.ContainsNgWord = masterData.CheckNgWords(request.player.playerName);

            var personalDbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await personalDbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            if (playerBasicInformation.playerName == request.player.playerName)
            {
                // todo: error message   conflict?
                throw new BadRequestException("requested player name is same as the registered");
            }

            var playerNameDuplicateCheck = await Common2DB.PlayerNames.FindAsync(playerBasicInformation.playerName);

            if (playerNameDuplicateCheck == null)
            {
                // todo: error message 以前にプレイヤー名変更に失敗している & 名前が再利用されていない
                throw new BadRequestException("player name update has failed");
            }

            if (playerNameDuplicateCheck.playerId != playerId)
            {
                // todo: error message 以前にプレイヤー名変更に失敗している & 名前が再利用されている
                throw new BadRequestException("player name update has failed & player name is reused");
            }

            var newPlayerName = new evolib.Databases.common2.PlayerName
            {
                playerName = request.player.playerName,
                playerId = playerId,
            };

            try
            {
                Common2DB.PlayerNames.Add(newPlayerName);
                Common2DB.PlayerNames.Remove(playerNameDuplicateCheck);

                await Common2DB.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (((MySqlException)e.InnerException)?.SqlState == "23000")
            {
                // todo: error message
                throw new ConflictException("requested player name is already in use");
            }

            // todo: comment transaction
            playerBasicInformation.playerName = request.player.playerName;

            await personalDbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerAppOptionResponse> GetPlayerAppOption(long playerId, int category)
        {
            if (!new[] { 0, 1, 3 }.Contains(category))
            {
                // todo: error message
                throw new BadRequestException($"invalid category: {category}");
            }

            var response = new GetPlayerAppOptionResponse();
            response.Options = new List<GetPlayerAppOptionResponse.Option>();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var registeredOption = await dbContext.AppOptions.SingleOrDefaultAsync(r => r.playerId == playerId && r.category == category);

            var settings = (_translationService.GetPlayerOptionSettings()).Options.Where(x => x.Category == category);

            foreach (var setting in settings)
            {
                var option = new GetPlayerAppOptionResponse.Option
                {
                    Category = setting.Category,
                    OptionNo = setting.Value,
                    JpText = setting.JpText,
                    EnText = setting.EnText,
                    ValueType = setting.ValueType,
                    IsLocal = setting.IsLocal,
                };

                var registeredValue = (int?)registeredOption?.GetType().GetProperty($"value{setting.Value}").GetValue(registeredOption);

                switch (setting.ValueType)
                {
                    case PlayerOptionSettings.ValueType.Switch:
                        var switchSetting = setting.Values.ToObject<SwitchSetting>();
                        option.SwitchSetting = new GetPlayerAppOptionResponse.SwitchSetting
                        {
                            Items = switchSetting.Items
                            .Select(x => new GetPlayerAppOptionResponse.SwitchItem
                            {
                                Index = x.Index,
                                JpText = x.JpText,
                                EnText = x.EnText,
                            })
                            .OrderBy(x => x.Index)
                            .ToList(),
                        };
                        option.Value = registeredValue ?? switchSetting.DefaultIndex;
                        break;
                    case PlayerOptionSettings.ValueType.Range:
                        var rangeSetting = setting.Values.ToObject<RangeSetting>();
                        option.RangeSetting = new GetPlayerAppOptionResponse.RangeSetting
                        {
                            Min = rangeSetting.Min,
                            Max = rangeSetting.Max,
                        };
                        option.Value = registeredValue ?? rangeSetting.Default;
                        break;
                    case PlayerOptionSettings.ValueType.Key:
                        var keySetting = setting.Values.ToObject<KeySetting>();
                        option.Value = registeredValue ?? keySetting.Default;
                        break;
                    default:
                        throw new Exception($"unexpected ValueType: {setting.ValueType}");
                }

                response.Options.Add(option);
            }

            return response;
        }

        public async Task<PutPlayerAppOptionResponse> PutPlayerAppOption(long playerId, int category, PutPlayerAppOptionRequest request)
        {
            if (!new[] { 0, 1, 3 }.Contains(category))
            {
                // todo: error message
                throw new BadRequestException($"invalid category: {category}");
            }

            var response = new PutPlayerAppOptionResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var settings = (_translationService.GetPlayerOptionSettings()).Options.Where(x => x.Category == category);

            if (!IsValidPlayerOptionValue(settings.FirstOrDefault(x => x.Value == request.OptionNo), null, request.Value))
            {
                // todo: error message
                throw new BadRequestException($"invalid parameter. category: {category}, optionNo: {request.OptionNo}, value: {request.Value}");
            }

            var registeredOption = await dbContext.AppOptions.FindAsync(playerId, category);

            if (registeredOption != null)
            {
                registeredOption.GetType().GetProperty($"value{request.OptionNo}").SetValue(registeredOption, request.Value);
            }
            else
            {
                var option = new AppOption
                {
                    category = category,
                    playerId = playerId,
                };

                foreach (var setting in settings)
                {
                    int tempValue;

                    switch (setting.ValueType)
                    {
                        case PlayerOptionSettings.ValueType.Switch:
                            tempValue = setting.Values.ToObject<SwitchSetting>().DefaultIndex;
                            break;
                        case PlayerOptionSettings.ValueType.Range:
                            tempValue = setting.Values.ToObject<RangeSetting>().Default;
                            break;
                        case PlayerOptionSettings.ValueType.Key:
                            tempValue = setting.Values.ToObject<KeySetting>().Default;
                            break;
                        default:
                            tempValue = 0;
                            break;
                    }

                    if (setting.Value == request.OptionNo) tempValue = request.Value;

                    option.GetType().GetProperty($"value{setting.Value}").SetValue(option, tempValue);

                    dbContext.AppOptions.Add(option);
                }
            }

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerMobileSuitOptionResponse> GetPlayerMobileSuitOption(long playerId, string mobileSuitId)
        {
            // todo: 共通化
            var mobileSuitIdCommon = "Common";

            if (mobileSuitId != mobileSuitIdCommon)
            {
                await MasterDataLoader.LoadAsync();

                var masterData = MasterDataLoader.LatestMasterData;

                if (masterData == null)
                {
                    // todo: error message
                    throw new Exception("master data not exist");
                }

                if (!masterData.AllMobileSuitIds.Contains(mobileSuitId))
                {
                    // todo: error message
                    throw new BadRequestException($"invalid mobileSuitId: {mobileSuitId}");
                }
            }

            var response = new GetPlayerMobileSuitOptionResponse();
            response.Options = new List<GetPlayerMobileSuitOptionResponse.Option>();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var registeredOption = await dbContext.MobileSuitOptions.SingleOrDefaultAsync(r => r.playerId == playerId && r.mobileSuitId == mobileSuitId);

            // todo: 定数化
            var settings = (_translationService.GetPlayerOptionSettings()).Options.Where(x => x.Category == 2);

            foreach (var setting in settings)
            {
                var option = new GetPlayerMobileSuitOptionResponse.Option
                {
                    OptionNo = setting.Value,
                    JpText = setting.JpText,
                    EnText = setting.EnText,
                    ValueType = setting.ValueType,
                    IsLocal = setting.IsLocal,
                };

                var registeredValue = (int?)registeredOption?.GetType().GetProperty($"value{setting.Value}").GetValue(registeredOption);

                switch (setting.ValueType)
                {
                    case PlayerOptionSettings.ValueType.Switch:
                        var switchSetting = setting.Values.ToObject<SwitchSetting>();
                        option.SwitchSetting = new GetPlayerMobileSuitOptionResponse.SwitchSetting
                        {
                            Items = switchSetting.Items
                            .Select(x => new GetPlayerMobileSuitOptionResponse.SwitchItem
                            {
                                Index = x.Index,
                                JpText = x.JpText,
                                EnText = x.EnText,
                            })
                            .OrderBy(x => x.Index)
                            .ToList(),
                        };
                        option.Value = registeredValue ?? switchSetting.DefaultIndex;
                        break;
                    case PlayerOptionSettings.ValueType.SwitchMs:
                        var switchMsSetting = setting.Values.ToObject<SwitchMsSetting>();
                        var switchMobileSuitSetting = switchMsSetting.Items.SingleOrDefault(x => x.MsId == mobileSuitId);

                        if (switchMobileSuitSetting == null) continue;

                        option.JpText = switchMobileSuitSetting.JpText;
                        option.EnText = switchMobileSuitSetting.EnText;
                        option.SwitchMsSetting = new GetPlayerMobileSuitOptionResponse.SwitchMsSetting
                        {
                            Items = switchMobileSuitSetting.Items
                            .Select(x => new GetPlayerMobileSuitOptionResponse.SwitchMsItem
                            {
                                Index = x.Index,
                                JpText = x.JpText,
                                EnText = x.EnText,
                            })
                            .OrderBy(x => x.Index)
                            .ToList(),
                            IsCommonSettingAvailable = mobileSuitId != mobileSuitIdCommon && switchMsSetting.Items.Any(x => x.MsId == mobileSuitIdCommon),
                        };
                        option.Value = registeredValue ?? switchMobileSuitSetting.Default;
                        break;
                    case PlayerOptionSettings.ValueType.Range:
                        var rangeSetting = setting.Values.ToObject<RangeSetting>();
                        option.RangeSetting = new GetPlayerMobileSuitOptionResponse.RangeSetting
                        {
                            Min = rangeSetting.Min,
                            Max = rangeSetting.Max,
                        };
                        option.Value = registeredValue ?? rangeSetting.Default;
                        break;
                    case PlayerOptionSettings.ValueType.RangeMs:
                        var rangeMsSetting = setting.Values.ToObject<RangeMsSetting>();
                        var rangeMobileSuitSetting = rangeMsSetting.Items.SingleOrDefault(x => x.MsId == mobileSuitId);

                        if (rangeMobileSuitSetting == null) continue;

                        option.JpText = rangeMobileSuitSetting.JpText;
                        option.EnText = rangeMobileSuitSetting.EnText;
                        option.RangeMsSetting = new GetPlayerMobileSuitOptionResponse.RangeMsSetting
                        {
                            Min = rangeMobileSuitSetting.Min,
                            Max = rangeMobileSuitSetting.Max,
                            IsCommonSettingAvailable = mobileSuitId != mobileSuitIdCommon && rangeMsSetting.Items.Any(x => x.MsId == mobileSuitIdCommon),
                        };
                        option.Value = registeredValue ?? rangeMobileSuitSetting.Default;
                        break;
                    case PlayerOptionSettings.ValueType.Key:
                        var keySetting = setting.Values.ToObject<KeySetting>();
                        option.Value = registeredValue ?? keySetting.Default;
                        break;
                    default:
                        throw new Exception($"unexpected ValueType: {setting.ValueType}");
                }

                response.Options.Add(option);
            }

            return response;
        }

        public async Task<PutPlayerMobileSuitOptionResponse> PutPlayerMobileSuitOption(long playerId, string mobileSuitId, PutPlayerMobileSuitOptionRequest request)
        {
            // todo: 共通化
            var mobileSuitIdCommon = "Common";

            if (mobileSuitId != mobileSuitIdCommon)
            {
                await MasterDataLoader.LoadAsync();

                var masterData = MasterDataLoader.LatestMasterData;

                if (masterData == null)
                {
                    // todo: error message
                    throw new Exception("master data not exist");
                }

                if (!masterData.AllMobileSuitIds.Contains(mobileSuitId))
                {
                    // todo: error message
                    throw new BadRequestException($"invalid mobileSuitId: {mobileSuitId}");
                }
            }

            var response = new PutPlayerMobileSuitOptionResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: 定数化
            var settings = (_translationService.GetPlayerOptionSettings()).Options.Where(x => x.Category == 2);

            if (!IsValidPlayerOptionValue(settings.FirstOrDefault(x => x.Value == request.OptionNo), mobileSuitId, request.Value))
            {
                // todo: error message
                throw new BadRequestException($"invalid parameter. optionNo: {request.OptionNo}, mobileSuitId: {mobileSuitId}, value: {request.Value}");
            }

            var registeredOption = await dbContext.MobileSuitOptions.FindAsync(playerId, mobileSuitId);

            if (registeredOption != null)
            {
                registeredOption.GetType().GetProperty($"value{request.OptionNo}").SetValue(registeredOption, request.Value);
            }
            else
            {
                var option = new MobileSuitOption
                {
                    playerId = playerId,
                    mobileSuitId = mobileSuitId,
                };

                foreach (var setting in settings)
                {
                    int tempValue;

                    switch (setting.ValueType)
                    {
                        case PlayerOptionSettings.ValueType.Switch:
                            tempValue = setting.Values.ToObject<SwitchSetting>().DefaultIndex;
                            break;
                        case PlayerOptionSettings.ValueType.SwitchMs:
                            var switchMobileSuitSetting = setting.Values.ToObject<SwitchMsSetting>().Items.SingleOrDefault(x => x.MsId == mobileSuitId);
                            tempValue = switchMobileSuitSetting == null ? 0 : switchMobileSuitSetting.Default;
                            break;
                        case PlayerOptionSettings.ValueType.Range:
                            tempValue = setting.Values.ToObject<RangeSetting>().Default;
                            break;
                        case PlayerOptionSettings.ValueType.RangeMs:
                            var rangeMobileSuitSetting = setting.Values.ToObject<RangeMsSetting>().Items.SingleOrDefault(x => x.MsId == mobileSuitId);
                            tempValue = rangeMobileSuitSetting == null ? 0 : rangeMobileSuitSetting.Default;
                            break;
                        case PlayerOptionSettings.ValueType.Key:
                            tempValue = setting.Values.ToObject<KeySetting>().Default;
                            break;
                        default:
                            tempValue = 0;
                            break;
                    }

                    if (setting.Value == request.OptionNo) tempValue = request.Value;

                    option.GetType().GetProperty($"value{setting.Value}").SetValue(option, tempValue);

                    dbContext.MobileSuitOptions.Add(option);
                }
            }

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerItemResponse> GetPlayerItem(long playerId)
        {
            var response = new GetPlayerItemResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            response.items = await GetPlayerItem(masterData, playerId);

            return response;
        }

        public async Task<PutPlayerItemResponse> PutPlayerItem(long playerId, PutPlayerItemRequest request)
        {
            var response = new PutPlayerItemResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInfo = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInfo == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: player の状態確認は必要？

            if (request.items.GroupBy(item => item.itemId).Any(item => item.Count() > 1))
            {
                // todo: error message
                throw new BadRequestException("duplicated itemId");
            }

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            if (request.items.Any(i => masterData.GetItemFromItemId(i.itemId) == null))
            {
                // todo: error message
                throw new BadRequestException("invalid itemId");
            }

            if (request.items.Any(i => masterData.CheckDefaultOwnedItem(i.itemId) && !i.owned))
            {
                // todo: error message
                throw new BadRequestException("default owned item");
            }

            var ownedItemInventories = await dbContext.ItemInventories.Where(i => i.playerId == playerId).ToListAsync();

            foreach (var requestItem in request.items)
            {
                if (masterData.CheckDefaultOwnedItem(requestItem.itemId))
                {
                    continue;
                }

                var masterItem = masterData.GetItemFromItemId(requestItem.itemId);

                if (masterItem == null)
                {
                    // todo: error message
                    throw new BadRequestException("invalid itemId");
                }

                var ownedItemInventory = ownedItemInventories.SingleOrDefault(i => i.itemId == requestItem.itemId);

                if (ownedItemInventory == null && requestItem.owned)
                {
                    await dbContext.ItemInventories.AddAsync(new ItemInventory()
                    {
                        playerId = playerId,
                        itemId = masterItem.itemId,
                        itemType = masterItem.itemType,
                        obtainedDate = DateTime.UtcNow,
                        obtainedWay = evolib.Item.ObtainedWay.Tool,
                        isNew = true, // todo: 用途を確認する
                    });
                }
                else if (ownedItemInventory != null && !requestItem.owned)
                {
                    dbContext.ItemInventories.Remove(ownedItemInventory);
                }
            }

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerPassResponse> GetPlayerPass(long playerId)
        {
            var response = new GetPlayerPassResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            response.battlePasses = await GetPlayerBattlePass(masterData, dbContext, playerId);

            return response;
        }

        public async Task<PutPlayerPassResponse> PutPlayerPass(long playerId, int passId, PutPlayerPassRequest request)
        {
            // todo: CBT後 バトルパス

            var response = new PutPlayerPassResponse();

            if (passId == evolib.BattlePass.PlayerLevelPassId)
            {
                // todo: error message
                throw new BadRequestException("battle pass not exist");
            }

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: player の状態確認は必要？

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var battlePassMaster = masterData.GetBattlePass(passId);

            if (battlePassMaster == null)
            {
                // todo: error message
                throw new BadRequestException("battle pass not exist");
            }

            // todo: insertも考慮する？

            var battlePass = await dbContext.BattlePasses.FindAsync(playerId, passId);

            battlePass.totalExp = request.battlePass.totalExp;
            battlePass.isPremium = request.battlePass.isPremium;
            battlePass.updatedDate = DateTime.UtcNow;
            battlePass.SetLevelDetail(masterData);

            // todo: CBT後 レベルアップ処理 報酬付与
            // todo: CBT後 無償→有償 有償バトルパス報酬獲得処理

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerExpResponse> GetPlayerExp(long playerId)
        {
            var response = new GetPlayerExpResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: player の状態確認は必要？

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            response.exp = await GetPlayerExp(dbContext, playerId);

            return response;
        }

        public async Task<PutPlayerExpResponse> PutPlayerExp(long playerId, PutPlayerExpRequest request)
        {
            var response = new PutPlayerExpResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: player の状態確認は必要？

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var battlePass = await dbContext.BattlePasses
                                            .FirstOrDefaultAsync(r => r.playerId == playerId &&
                                                                      r.passId == evolib.BattlePass.PlayerLevelPassId);

            if (battlePass == null)
            {
                // todo: error message
                throw new NotFoundException("player level data not exist");
            }

            battlePass.totalExp = request.exp.totalExp;
            battlePass.updatedDate = DateTime.UtcNow;
            battlePass.SetLevelDetail(masterData);

            // todo: CBT後 レベルアップ処理 実績付与

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerTutorialResponse> GetPlayerTutorial(long playerId)
        {
            var playerBasicInformation = await PDBSM.PersonalDBContext(playerId)
                                                    .PlayerBasicInformations
                                                    .FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var dictionary = _translationService.GetTutorialDictionary();

            TranslationItemCommon translationItem;
            dictionary.TryGetValue(playerBasicInformation.tutorialProgress.ToString(), out translationItem);

            return new GetPlayerTutorialResponse
            {
                tutorial = new GetPlayerTutorialResponse.Tutorial
                {
                    isFirstTutorialEnd = 0 < (playerBasicInformation.initialLevel & PlayerInformation.InitialLevelFlg.Tutorial),
                    tutorialProgress = playerBasicInformation.tutorialProgress,
                    displayNameJapanese = translationItem?.JpText ?? playerBasicInformation.tutorialProgress.ToString(),
                    displayNameEnglish = translationItem?.EnText ?? playerBasicInformation.tutorialProgress.ToString(),
                }
            };
        }

        public async Task<PutPlayerTutorialResponse> PutPlayerTutorial(long playerId, PutPlayerTutorialRequest request)
        {
            var response = new PutPlayerTutorialResponse();

            var personalDbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await personalDbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            if (request.player.isFirstTutorialEnd)
            {
                playerBasicInformation.initialLevel |= PlayerInformation.InitialLevelFlg.Tutorial;
            }
            else
            {
                playerBasicInformation.initialLevel &= ~PlayerInformation.InitialLevelFlg.Tutorial;
            }

            await personalDbContext.SaveChangesAsync();

            return response;
        }

        public async Task<PutPlayerTutorialResetResponse> PutPlayerTutorialReset(long playerId)
        {
            var response = new PutPlayerTutorialResetResponse();

            var personalDbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await personalDbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            playerBasicInformation.tutorialProgress = 0;

            await personalDbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetPlayerCareerRecordResponse> GetPlayerCareerRecord(long playerId, int seasonNo, string mobileSuitId)
        {
            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            // todo: player の状態確認は必要？

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var dictionary = _translationService.GetCareerRecordDictionary();

            var query = dbContext.CareerRecords.Where(r => r.playerId == playerId);

            if (seasonNo == 0)
            {
                query = query.Where(r => r.matchType == evolib.Battle.MatchType.Casual && r.seasonNo == seasonNo);
            }
            else
            {
                query = query.Where(r => r.matchType == evolib.Battle.MatchType.Rank && r.seasonNo == seasonNo);
            }

            query = query.Where(r => r.mobileSuitId == mobileSuitId);

            var careerRecords = await query.OrderBy(r => r.recordItemId).ToListAsync();

            var response = new GetPlayerCareerRecordResponse();
            response.careerRecords = new List<GetPlayerCareerRecordResponse.CareerRecord>();

            foreach (var record in careerRecords)
            {
                TranslationItemCommon translationItem;
                dictionary.TryGetValue(record.recordItemId, out translationItem);

                response.careerRecords.Add(new GetPlayerCareerRecordResponse.CareerRecord
                {
                    careerRecordId = record.Id,
                    recordItemId = record.recordItemId,
                    displayNameJapanese = translationItem?.JpText ?? record.recordItemId,
                    displayNameEnglish = translationItem?.EnText ?? record.recordItemId,
                    value = record.value,
                    numForAverage = record.numForAverage,
                });
            }

            return response;
        }

        public async Task<PutPlayerCareerRecordResponse> PutPlayerCareerRecord(long playerId, long careerRecordId, PutPlayerCareerRecordRequest request)
        {
            var response = new PutPlayerCareerRecordResponse();

            var dbContext = PDBSM.PersonalDBContext(playerId);

            var playerBasicInformation = await dbContext.PlayerBasicInformations.FindAsync(playerId);

            if (playerBasicInformation == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var careerRecord = await dbContext.CareerRecords.FindAsync(careerRecordId);

            if (careerRecord == null)
            {
                // todo: error message
                throw new NotFoundException("careerrecord not exist");
            }

            if (careerRecord.playerId != playerId)
            {
                // todo: error message
                throw new BadRequestException("invalid request");
            }

            careerRecord.value = request.value;
            careerRecord.numForAverage = request.numForAverage;

            await dbContext.SaveChangesAsync();

            return response;
        }

        private async Task<IList<GetPlayerItemResponse.Item>> GetPlayerItem(IMasterData masterData, long playerId)
        {
            var response = new List<GetPlayerItemResponse.Item>();

            var ownedItemInventories = await PDBSM.PersonalDBContext(playerId)
                                      .ItemInventories
                                      .Where(r => r.playerId == playerId)
                                      .ToListAsync();

            var dictionary = _translationService.GetItemDictionary();

            foreach (var itemId in masterData.AllItemIds)
            {
                var item = masterData.GetItemFromItemId(itemId);

                var isDefaultItem = masterData.CheckDefaultOwnedItem(item.itemId);

                TranslationItemCommon translationItem;
                dictionary.TryGetValue(itemId, out translationItem);

                response.Add(new GetPlayerItemResponse.Item
                {
                    itemId = item.itemId,
                    itemType = item.itemType.ToString(),
                    displayNameJapanese = translationItem?.JpText ?? item.itemId,
                    displayNameEnglish = translationItem?.EnText ?? item.itemId,
                    owned = isDefaultItem || ownedItemInventories.Exists(r => r.itemId == item.itemId),
                    isDefault = isDefaultItem,
                });
            }

            return response;
        }

        private async Task<IList<GetPlayerPassResponse.BattlePass>> GetPlayerBattlePass(IMasterData masterData, PersonalDBContext dbContext, long playerId)
        {
            var response = new List<GetPlayerPassResponse.BattlePass>();

            var dictionary = _translationService.GetSeasonDictionary();

            var battlePasses = await dbContext.BattlePasses
                                              .Where(r => r.playerId == playerId &&
                                                          r.passId != evolib.BattlePass.PlayerLevelPassId)
                                              .ToListAsync();

            foreach (var battlePass in battlePasses)
            {
                var battlePassMaster = masterData.GetBattlePass(battlePass.passId);

                if (battlePassMaster == null) continue;

                TranslationItemCommon translationItem;
                dictionary.TryGetValue(battlePassMaster.seasonNo.ToString(), out translationItem);

                response.Add(new GetPlayerPassResponse.BattlePass
                {
                    id = battlePass.passId,
                    displayNameJapanese = translationItem?.JpText ?? battlePass.passId.ToString(),
                    displayNameEnglish = translationItem?.EnText ?? battlePass.passId.ToString(),
                    totalExp = battlePass.totalExp,
                    isPremium = battlePass.isPremium,
                    level = battlePass.level,
                    rewardLevel = battlePass.rewardLevel,
                    levelExp = battlePass.levelExp,
                    nextExp = battlePass.nextExp,
                    createdDate = battlePass.createdDate,
                    updatedDate = battlePass.updatedDate,
                });
            }

            return response;
        }

        private async Task<GetPlayerExpResponse.Exp> GetPlayerExp(PersonalDBContext dbContext, long playerId)
        {
            var battlePass = await dbContext.BattlePasses
                                            .FirstOrDefaultAsync(r => r.playerId == playerId &&
                                                                      r.passId == evolib.BattlePass.PlayerLevelPassId);

            if (battlePass == null)
            {
                // todo: error message
                throw new NotFoundException("player level data not exist");
            }

            return new GetPlayerExpResponse.Exp
            {
                totalExp = battlePass.totalExp,
                level = battlePass.level,
                rewardLevel = battlePass.rewardLevel,
                levelExp = battlePass.levelExp,
                nextExp = battlePass.nextExp,
                createdDate = battlePass.createdDate,
                updatedDate = battlePass.updatedDate,
            };
        }

        private bool IsValidPlayerOptionValue(PlayerOptionSetting setting, string mobileSuitId, int value)
        {
            // todo: 共通化
            var mobileSuitIdCommon = "Common";

            if (setting == null)
            {
                return false;
            }

            switch (setting.ValueType)
            {
                case PlayerOptionSettings.ValueType.Switch:
                    var switchSetting = setting.Values.ToObject<SwitchSetting>();
                    return switchSetting.Items.Any(x => x.Index == value);
                case PlayerOptionSettings.ValueType.SwitchMs:
                    var switchMsSetting = setting.Values.ToObject<SwitchMsSetting>();
                    var switchMobileSuitSetting = switchMsSetting.Items.SingleOrDefault(x => x.MsId == mobileSuitId);

                    if (switchMobileSuitSetting != null)
                    {
                        if (switchMobileSuitSetting.Items.Any(x => x.Index == value))
                        {
                            return true;
                        }

                        if (mobileSuitId != mobileSuitIdCommon && value == -1 && switchMsSetting.Items.Any(x => x.MsId == mobileSuitIdCommon))
                        {
                            return true;
                        }
                    }

                    return false;
                case PlayerOptionSettings.ValueType.Range:
                    var rangeSetting = setting.Values.ToObject<RangeSetting>();
                    return rangeSetting.Min <= value && value <= rangeSetting.Max;
                case PlayerOptionSettings.ValueType.RangeMs:
                    var rangeMsSetting = setting.Values.ToObject<RangeMsSetting>();
                    var rangeMobileSuitSetting = rangeMsSetting.Items.SingleOrDefault(x => x.MsId == mobileSuitId);

                    if (rangeMobileSuitSetting != null)
                    {
                        if (rangeMobileSuitSetting.Min <= value && value <= rangeMobileSuitSetting.Max)
                        {
                            return true;
                        }

                        if (mobileSuitId != mobileSuitIdCommon && value == -1 && rangeMsSetting.Items.Any(x => x.MsId == mobileSuitIdCommon))
                        {
                            return true;
                        }
                    }

                    return false;
                case PlayerOptionSettings.ValueType.Key:
                    return true;
                default:
                    throw new Exception($"unexpected ValueType: {setting.ValueType}");
            }
        }
    }
}
