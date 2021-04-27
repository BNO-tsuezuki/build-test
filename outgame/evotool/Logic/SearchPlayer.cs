using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;

namespace evotool.Logic
{
    public static class SearchPlayer
    {
		public static async Task<PlayerBasicInformation> ByPlayerName(string playerName,
			PersonalDBShardManager pdbsm, Common2DBContext common2DB )
		{
			var rec = await common2DB.PlayerNames.FindAsync(playerName);
			if (rec == null) return null;

			var db = pdbsm.PersonalDBContext(rec.playerId);
			return await db.PlayerBasicInformations.FindAsync(rec.playerId);
		}

		public static async Task<PlayerBasicInformation> ByAccount(string account,
			PersonalDBShardManager pdbsm, Common1DBContext common1DB )
		{
			var r = await common1DB.Accounts.FindAsync(account, evolib.Account.Type.Dev1);
			if (r == null) return null;

			return await pdbsm.PersonalDBContext(r.playerId).PlayerBasicInformations.FindAsync(r.playerId);
		}

	}
}
