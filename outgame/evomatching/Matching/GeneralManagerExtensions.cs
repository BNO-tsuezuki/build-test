using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public static class GeneralManagerExtensions
	{
		public static evolib.Kvs.Models.ConnectionQueueData.PremadeGroup CreatePremadeGroupQueueData( 
			this GeneralManager gm,
			long playerId
		)
		{
			var ret = new evolib.Kvs.Models.ConnectionQueueData.PremadeGroup();
			ret.players = new List<evolib.Kvs.Models.ConnectionQueueData.PremadeGroup.Player>();

			gm.PgInvitationManager.GetInvitationFrom(playerId).ForEach(inv =>
			{
				ret.players.Add(new evolib.Kvs.Models.ConnectionQueueData.PremadeGroup.Player
				{
					playerId = inv.PlayerDst.PlayerId,
					isInvitation = true,
					remainingSec = (float)((inv.Expiry - DateTime.UtcNow).TotalSeconds),
					expirySec = (float)(evolib.PremadeGroup.InvitationExpiry.TotalSeconds),
				});
			});
			

			var group = gm.PremadeGroupManager.GetBelongs(playerId);
			if( group == null) return ret;

			for (int i = 0; i < group.Players.Count; i++)
			{
				var player = group.Players[i];

				ret.players.Add(new evolib.Kvs.Models.ConnectionQueueData.PremadeGroup.Player
				{
					playerId = player.PlayerId,
					isLeader = (group.LeaderPlayerId == player.PlayerId),
				});

				if (player.PlayerId == playerId) continue;

				gm.PgInvitationManager.GetInvitationFrom(player.PlayerId).ForEach(inv =>
				{
					ret.players.Add(new evolib.Kvs.Models.ConnectionQueueData.PremadeGroup.Player
					{
						playerId = inv.PlayerDst.PlayerId,
						isInvitation = true,
						remainingSec = (float)((inv.Expiry - DateTime.UtcNow).TotalSeconds),
						expirySec = (float)(evolib.PremadeGroup.InvitationExpiry.TotalSeconds),
					});
				});
			}

			return ret;
		}
	}
}
