using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evotool.Models;
using evotool.ProtocolModels.GMTool.AccountApi;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.GMTool
{
    public interface IAccountService
    {
        Task<GetAccountResponse> GetAccount(long playerId);
        Task<PutAccountResponse> PutAccount(long playerId, PutAccountRequest request);
        Task<GetAccountPrivilegeLevelResponse> GetAccountPrivilegeLevel(long playerId);
        Task<PutAccountPrivilegeLevelResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request);
        Task<PutResetAccountResponse> PutResetAccount(long playerId);
    }

    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IServicePack servicePack) : base(servicePack)
        { }

        public async Task<GetAccountResponse> GetAccount(long playerId)
        {
            var response = new GetAccountResponse();

            var registeredAccount = await Common1DB.PlayerIds
                                                   .Where(r => r.playerId == playerId)
                                                   .Join(Common1DB.Accounts,
                                                         p => new { AccountType = p.accountType, p.account },
                                                         a => new { AccountType = a.type, a.account },
                                                         (p, a) => a)
                                                   .FirstOrDefaultAsync();

            if (registeredAccount == null)
            {
                // todo: error message
                throw new NotFoundException("account not exist");
            }

            response.account = new GetAccountResponse.Account
            {
                account = registeredAccount.account,
                type = (int)registeredAccount.type,
                playerId = registeredAccount.playerId,
                privilegeLevel = registeredAccount.privilegeLevel,
                inserted = registeredAccount.inserted,
                banExpiration = registeredAccount.banExpiration,
            };

            return response;
        }

        public async Task<PutAccountResponse> PutAccount(long playerId, PutAccountRequest request)
        {
            var response = new PutAccountResponse();

            var registeredAccount = await Common1DB.PlayerIds
                                                   .Where(r => r.playerId == playerId)
                                                   .Join(Common1DB.Accounts,
                                                         p => new { AccountType = p.accountType, p.account },
                                                         a => new { AccountType = a.type, a.account },
                                                         (p, a) => a)
                                                   .FirstOrDefaultAsync();

            if (registeredAccount == null)
            {
                // todo: error message
                throw new NotFoundException("account not exist");
            }

            registeredAccount.banExpiration = request.account.banExpiration;

            await Common1DB.SaveChangesAsync();

            return response;
        }

        public async Task<GetAccountPrivilegeLevelResponse> GetAccountPrivilegeLevel(long playerId)
        {
            var player = await Common1DB.PlayerIds.FindAsync(playerId);

            if (player == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var account = await Common1DB.Accounts.FindAsync(player.account, player.accountType);

            if (account == null)
            {
                // todo: error message
                throw new NotFoundException("account not exist");
            }

            return new GetAccountPrivilegeLevelResponse
            {
                account = new GetAccountPrivilegeLevelResponse.Account
                {
                    isCheatCommandAvailable = 0 < (account.privilegeLevel & (1 << (int)evolib.Privilege.Type.CheatCommand)),
                },
            };
        }

        public async Task<PutAccountPrivilegeLevelResponse> PutAccountPrivilegeLevel(long playerId, PutAccountPrivilegeLevelRequest request)
        {
            var player = await Common1DB.PlayerIds.FindAsync(playerId);

            if (player == null)
            {
                // todo: error message
                throw new NotFoundException("player not exist");
            }

            var account = await Common1DB.Accounts.FindAsync(player.account, player.accountType);

            if (account == null)
            {
                // todo: error message
                throw new NotFoundException("account not exist");
            }

            if (request.account.isCheatCommandAvailable)
            {
                account.privilegeLevel |= (1 << (int)evolib.Privilege.Type.CheatCommand);
            }
            else
            {
                account.privilegeLevel &= ~(1 << (int)evolib.Privilege.Type.CheatCommand);
            }

            await Common1DB.SaveChangesAsync();

            var response = new PutAccountPrivilegeLevelResponse();

            return response;
        }

        public async Task<PutResetAccountResponse> PutResetAccount(long playerId)
        {
            var response = new PutResetAccountResponse();

            var registeredAccount = await Common1DB.PlayerIds
                                                   .Where(r => r.playerId == playerId)
                                                   .Join(Common1DB.Accounts,
                                                         p => new { AccountType = p.accountType, p.account },
                                                         a => new { AccountType = a.type, a.account },
                                                         (p, a) => a)
                                                   .FirstOrDefaultAsync();

            if (registeredAccount == null)
            {
                // todo: error message
                throw new NotFoundException("account not exist");
            }

			{//Personal
				var personalDB = PDBSM.PersonalDBContext(registeredAccount.playerId);

				//{
				//	var list = await personalDB.XXXX.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
				//	personalDB.XXXX.RemoveRange(list);
				//}

				{
					var list = await personalDB.PlayerBasicInformations.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.PlayerBasicInformations.RemoveRange(list);
				}
				{
					var list = await personalDB.PlayerBattleInformations.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.PlayerBattleInformations.RemoveRange(list);
				}
				{
					var list = await personalDB.OwnMobileSuitSettings.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.OwnMobileSuitSettings.RemoveRange(list);
				}
				{
					var list = await personalDB.OwnVoicePackSettings.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.OwnVoicePackSettings.RemoveRange(list);
				}
				{
					var list = await personalDB.ItemInventories.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.ItemInventories.RemoveRange(list);
				}
				{
					var list = await personalDB.AssetsInventories.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.AssetsInventories.RemoveRange(list);
				}
				{
					var list = await personalDB.FavoriteFriends.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.FavoriteFriends.RemoveRange(list);
				}
				{
					var list = await personalDB.FriendRequests.Where(r => r.playerIdSrc == registeredAccount.playerId).ToListAsync();
					personalDB.FriendRequests.RemoveRange(list);
				}
				{
					var list = await personalDB.MutePlayers.Where(r => r.playerIdSrc == registeredAccount.playerId).ToListAsync();
					personalDB.MutePlayers.RemoveRange(list);
				}
				{
					var list = await personalDB.AppOptions.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.AppOptions.RemoveRange(list);
				}
				{
					var list = await personalDB.MobileSuitOptions.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.MobileSuitOptions.RemoveRange(list);
				}
				{
					var list = await personalDB.DateLogs.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.DateLogs.RemoveRange(list);
				}
				{
					var list = await personalDB.ReplayUserHistory.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.ReplayUserHistory.RemoveRange(list);
				}
				{
					var list = await personalDB.BattlePasses.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.BattlePasses.RemoveRange(list);
				}
				{
					var list = await personalDB.CareerRecords.Where(r => r.playerId == registeredAccount.playerId).ToListAsync();
					personalDB.CareerRecords.RemoveRange(list);
				}

				await personalDB.SaveChangesAsync();
			}
			{//Common3

				//await Common3DB.SaveChangesAsync();
			}
			{//Common2
				{
					var list = await Common2DB.Friends.Where(r => r.playerIdL == registeredAccount.playerId
																|| r.playerIdR == registeredAccount.playerId).ToListAsync();
					Common2DB.Friends.RemoveRange(list);
				}
				await Common2DB.SaveChangesAsync();
			}
			{//Common1
				{
					var rec = new evolib.Databases.common1.PlayerId
					{
						playerId = registeredAccount.playerId
					};
					Common1DB.PlayerIds.Attach(rec);
					Common1DB.PlayerIds.Remove(rec);
				}
				{
					Common1DB.Accounts.Remove(registeredAccount);
				}

				await Common1DB.SaveChangesAsync();
			}
			
			return response;
        }
    }
}
