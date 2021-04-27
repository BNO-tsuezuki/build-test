using System.Linq;
using System.Threading.Tasks;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evolib.Databases.personal;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.Gdpr
{
    public interface IGdprService
    {
        Task<long?> GetPlayerId(string account);
        Task<object> GetAggregationData(string account);
    }

    public class GdprService : IGdprService
    {
        private readonly PersonalDBShardManager PDBSM;
        private readonly Common1DBContext Common1DB;
        private readonly Common2DBContext Common2DB;
        private readonly Common3DBContext Common3DB;
        private PersonalDBContext PDB;

        public GdprService(IServicePack servicePack)
        {
            PDBSM = servicePack.PersonalDBShardManager;
            Common1DB = servicePack.Common1DBContext;
            Common2DB = servicePack.Common2DBContext;
            Common3DB = servicePack.Common3DBContext;
        }

        public async Task<long?> GetPlayerId(string account)
        {
            var temp = await Common1DB.Accounts
                .SingleOrDefaultAsync(x => x.account == account && x.type == evolib.Account.Type.Inky);

            return temp?.playerId;
        }

        public async Task<object> GetAggregationData(string account)
        {
            var playerId = await GetPlayerId(account);

            if (playerId == null)
            {
                return null;
            }

            PDB = PDBSM.PersonalDBContext(playerId.Value);

            // evocommon1
            var accounts  = await GetAccountsAsync(account);
            var playerIds = await GetPlayerIdsAsync(playerId.Value);

            // evocommon2

            // evocommon3

            // evopersonal
            var achievements             = await GetAchievementsAsync(playerId.Value);
            var appOptions               = await GetAppOptionsAsync(playerId.Value);
            var assetsInventories        = await GetAssetsInventoriesAsync(playerId.Value);
            var battlePasses             = await GetBattlePassesAsync(playerId.Value);
            var careerRecords            = await GetCareerRecordsAsync(playerId.Value);
            var dateLogs                 = await GetDateLogsAsync(playerId.Value);
            var itemInventories          = await GetItemInventoriesAsync(playerId.Value);
            var mobileSuitOptions        = await GetMobileSuitOptionsAsync(playerId.Value);
            var ownMobileSuitSettings    = await GetOwnMobileSuitSettingsAsync(playerId.Value);
            var ownVoicePackSettings     = await GetOwnVoicePackSettingsAsync(playerId.Value);
            var playerBasicInformations  = await GetPlayerBasicInformationsAsync(playerId.Value);
            var playerBattleInformations = await GetPlayerBattleInformationsAsync(playerId.Value);
            var replayUserHistory        = await GetReplayUserHistoryAsync(playerId.Value);

            return new
            {
                accounts,
                playerIds,

                achievements,
                appOptions,
                assetsInventories,
                battlePasses,
                careerRecords,
                dateLogs,
                itemInventories,
                mobileSuitOptions,
                ownMobileSuitSettings,
                ownVoicePackSettings,
                playerBasicInformations,
                playerBattleInformations,
                replayUserHistory,
            };
        }

        // evocommon1

        private async Task<object> GetAccountsAsync(string account)
        {
            var temp = await Common1DB.Accounts
                .SingleOrDefaultAsync(x => x.account == account && x.type == evolib.Account.Type.Inky);

            if (temp == null)
            {
                return null;
            }

            return new
            {
                temp.account,
                temp.inserted,
                temp.privilegeLevel,
                temp.type,
                temp.banExpiration,
                temp.countryCode,
            };
        }

        private async Task<object> GetPlayerIdsAsync(long playerId)
        {
            var temp = await Common1DB.PlayerIds
                .SingleOrDefaultAsync(x => x.playerId == playerId);

            if (temp == null)
            {
                return null;
            }

            return new
            {
                temp.account,
                temp.accountType,
                temp.inserted,
            };
        }

        // evocommon2

        // evocommon3

        // evopersonal

        private async Task<object> GetAchievementsAsync(long playerId)
        {
            return await PDB.Achievements
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.achievementId,
                    x.value,
                    x.notified,
                    x.obtained,
                    x.obtainedDate,
                })
                .ToListAsync();
        }

        private async Task<object> GetAppOptionsAsync(long playerId)
        {
            return await PDB.AppOptions
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.category,
                    x.value0,
                    x.value1,
                    x.value2,
                    x.value3,
                    x.value4,
                    x.value5,
                    x.value6,
                    x.value7,
                    x.value8,
                    x.value9,
                    x.value10,
                    x.value11,
                    x.value12,
                    x.value13,
                    x.value14,
                    x.value15,
                    x.value16,
                    x.value17,
                    x.value18,
                    x.value19,
                    x.value20,
                    x.value21,
                    x.value22,
                    x.value23,
                    x.value24,
                    x.value25,
                    x.value26,
                    x.value27,
                    x.value28,
                    x.value29,
                    x.value30,
                    x.value31,
                    x.value32,
                    x.value33,
                    x.value34,
                    x.value35,
                    x.value36,
                    x.value37,
                    x.value38,
                    x.value39,
                    x.value40,
                    x.value41,
                    x.value42,
                    x.value43,
                    x.value44,
                    x.value45,
                    x.value46,
                    x.value47,
                    x.value48,
                    x.value49,
                })
                .ToListAsync();
        }

        private async Task<object> GetAssetsInventoriesAsync(long playerId)
        {
            return await PDB.AssetsInventories
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.assetsId,
                    x.amount,
                })
                .ToListAsync();
        }

        private async Task<object> GetBattlePassesAsync(long playerId)
        {
            return await PDB.BattlePasses
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.passId,
                    x.totalExp,
                    x.isPremium,
                    x.level,
                    x.rewardLevel,
                    x.levelExp,
                    x.nextExp,
                    x.createdDate,
                    x.updatedDate,
                })
                .ToListAsync();
        }

        private async Task<object> GetCareerRecordsAsync(long playerId)
        {
            return await PDB.CareerRecords
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.matchType,
                    x.seasonNo,
                    x.recordItemId,
                    x.mobileSuitId,
                    x.value,
                    x.numForAverage,
                })
                .ToListAsync();
        }

        private async Task<object> GetDateLogsAsync(long playerId)
        {
            return await PDB.DateLogs
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.OnlineStamp,
                    x.FriendRequestPageLastView,
                })
                .ToListAsync();
        }

        private async Task<object> GetItemInventoriesAsync(long playerId)
        {
            return await PDB.ItemInventories
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.itemType,
                    x.itemId,
                    x.obtainedDate,
                    x.obtainedWay,
                    x.isNew,
                })
                .ToListAsync();
        }

        private async Task<object> GetMobileSuitOptionsAsync(long playerId)
        {
            return await PDB.MobileSuitOptions
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.mobileSuitId,
                    x.value0,
                    x.value1,
                    x.value2,
                    x.value3,
                    x.value4,
                    x.value5,
                    x.value6,
                    x.value7,
                    x.value8,
                    x.value9,
                    x.value10,
                    x.value11,
                    x.value12,
                    x.value13,
                    x.value14,
                    x.value15,
                    x.value16,
                    x.value17,
                    x.value18,
                    x.value19,
                    x.value20,
                    x.value21,
                    x.value22,
                    x.value23,
                    x.value24,
                    x.value25,
                    x.value26,
                    x.value27,
                    x.value28,
                    x.value29,
                    x.value30,
                    x.value31,
                    x.value32,
                    x.value33,
                    x.value34,
                    x.value35,
                    x.value36,
                    x.value37,
                    x.value38,
                    x.value39,
                    x.value40,
                    x.value41,
                    x.value42,
                    x.value43,
                    x.value44,
                    x.value45,
                    x.value46,
                    x.value47,
                    x.value48,
                    x.value49,
                    x.value50,
                    x.value51,
                    x.value52,
                    x.value53,
                    x.value54,
                    x.value55,
                    x.value56,
                    x.value57,
                    x.value58,
                    x.value59,
                    x.value60,
                    x.value61,
                    x.value62,
                    x.value63,
                    x.value64,
                    x.value65,
                    x.value66,
                    x.value67,
                    x.value68,
                    x.value69,
                    x.value70,
                    x.value71,
                    x.value72,
                    x.value73,
                    x.value74,
                    x.value75,
                    x.value76,
                    x.value77,
                    x.value78,
                    x.value79,
                    x.value80,
                    x.value81,
                    x.value82,
                    x.value83,
                    x.value84,
                    x.value85,
                    x.value86,
                    x.value87,
                    x.value88,
                    x.value89,
                    x.value90,
                    x.value91,
                    x.value92,
                    x.value93,
                    x.value94,
                    x.value95,
                    x.value96,
                    x.value97,
                    x.value98,
                    x.value99,
                    x.value100,
                    x.value101,
                    x.value102,
                    x.value103,
                    x.value104,
                    x.value105,
                    x.value106,
                    x.value107,
                    x.value108,
                    x.value109,
                    x.value110,
                    x.value111,
                    x.value112,
                    x.value113,
                    x.value114,
                    x.value115,
                    x.value116,
                    x.value117,
                    x.value118,
                    x.value119,
                    x.value120,
                    x.value121,
                    x.value122,
                    x.value123,
                    x.value124,
                    x.value125,
                    x.value126,
                    x.value127,
                    x.value128,
                    x.value129,
                    x.value130,
                    x.value131,
                    x.value132,
                    x.value133,
                    x.value134,
                    x.value135,
                    x.value136,
                    x.value137,
                    x.value138,
                    x.value139,
                })
                .ToListAsync();
        }

        private async Task<object> GetOwnMobileSuitSettingsAsync(long playerId)
        {
            return await PDB.OwnMobileSuitSettings
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.mobileSuitId,
                    x.voicePackItemId,
                    x.ornamentItemId,
                    x.bodySkinItemId,
                    x.weaponSkinItemId,
                    x.mvpCelebrationItemId,
                    x.stampSlotItemId1,
                    x.stampSlotItemId2,
                    x.stampSlotItemId3,
                    x.stampSlotItemId4,
                    x.emotionSlotItemId1,
                    x.emotionSlotItemId2,
                    x.emotionSlotItemId3,
                    x.emotionSlotItemId4,
                })
                .ToListAsync();
        }

        private async Task<object> GetOwnVoicePackSettingsAsync(long playerId)
        {
            return await PDB.OwnVoicePackSettings
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.mobileSuitId,
                    x.voicePackItemId,
                    x.voiceId1,
                    x.voiceId2,
                    x.voiceId3,
                    x.voiceId4,
                })
                .ToListAsync();
        }

        private async Task<object> GetPlayerBasicInformationsAsync(long playerId)
        {
            var temp = await PDB.PlayerBasicInformations
                .SingleOrDefaultAsync(x => x.playerId == playerId);

            if (temp == null)
            {
                return null;
            }

            return new
            {
                temp.firstLogin,
                temp.playerName,
                temp.initialLevel,
                temp.playerIconItemId,
                temp.pretendOffline,
                temp.openType,
                temp.tutorialProgress,
            };
        }

        private async Task<object> GetPlayerBattleInformationsAsync(long playerId)
        {
            var temp = await PDB.PlayerBattleInformations
                .SingleOrDefaultAsync(x => x.playerId == playerId);

            if (temp == null)
            {
                return null;
            }

            return new
            {
                temp.rating,
                temp.victory,
                temp.defeat,
                temp.draw,
            };
        }

        private async Task<object> GetReplayUserHistoryAsync(long playerId)
        {
            return await PDB.ReplayUserHistory
                .Where(x => x.playerId == playerId)
                .Select(x => new
                {
                    x.date,
                    x.matchId,
                    x.matchType,
                    x.resultType,
                    x.packageVersion,
                    x.masterDataVersion,
                })
                .ToListAsync();
        }
    }
}
