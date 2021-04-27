using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using evolib.Log;


namespace evomatching.Matching
{
	public class PgInvitationManager
	{
		class PremadeGroupPlayer : IPremadeGroupPlayer
		{
			public long PlayerId { get; set; }
			public string SessionId { get; set; }
		}

		class PgInvitation : IPgInvitation
		{
			public IPremadeGroupPlayer PlayerSrc { get; set; }
			public IPremadeGroupPlayer PlayerDst { get; set; }

			public DateTime Expiry { get; set; }
		}



		Dictionary<long, PgInvitation> InvitationsDstMap
			= new Dictionary<long, PgInvitation>();
		Dictionary<long, Dictionary<long, PgInvitation>> InvitationsSrcMap
			= new Dictionary<long, Dictionary<long, PgInvitation>>();


		public bool CreateInvitation(
			long playerIdSrc, string sessionIdSrc,
			long playerIdDst, string sessionIdDst
		)
		{
			if (playerIdSrc == playerIdDst) return false;

			var inv = new PgInvitation
			{
				PlayerSrc = new PremadeGroupPlayer
				{
					PlayerId = playerIdSrc,
					SessionId = sessionIdSrc,
				},

				PlayerDst = new PremadeGroupPlayer
				{
					PlayerId = playerIdDst,
					SessionId = sessionIdDst,
				},

				Expiry = DateTime.UtcNow + evolib.PremadeGroup.InvitationExpiry,
			};

			InvitationsDstMap[playerIdDst] = inv;

			if (!InvitationsSrcMap.ContainsKey(playerIdSrc))
			{
				InvitationsSrcMap[playerIdSrc] = new Dictionary<long, PgInvitation>();
			}
			InvitationsSrcMap[playerIdSrc][playerIdDst] = inv;

			return true;
		}

		public void RemoveInvitation(IPgInvitation inv)
		{
			InvitationsDstMap.Remove(inv.PlayerDst.PlayerId);

			InvitationsSrcMap[inv.PlayerSrc.PlayerId].Remove(inv.PlayerDst.PlayerId);
		}

		public IPgInvitation GetInvitationTo(long playerId)
		{
			if (InvitationsDstMap.ContainsKey(playerId))
			{
				var inv = InvitationsDstMap[playerId];
				if (DateTime.UtcNow < inv.Expiry)
				{
					return inv;
				}
			}

			return null;
		}

		public List<IPgInvitation> GetInvitationFrom(long playerId)
		{
			var ret = new List<IPgInvitation>();

			if (InvitationsSrcMap.ContainsKey(playerId))
			{
				foreach (var inv in InvitationsSrcMap[playerId].Values)
				{
					if (DateTime.UtcNow < inv.Expiry)
					{
						ret.Add(inv);
					}
				}
			}

			return ret;
		}



		public void ClearExpiredInvitations()
		{
			var now = DateTime.UtcNow;

			//期限切れ招待状の削除
				
			var timeupInvs = new List<IPgInvitation>();
			foreach (var inv in InvitationsDstMap.Values)
			{
				if (inv.Expiry < now)
				{
					timeupInvs.Add(inv);

                    Logger.Logging(
                        new LogObj().AddChild(new LogModels.ResponseInvitationParty
                        {
                            PlayerIdSrc = inv.PlayerSrc.PlayerId,
                            PlayerIdDst = inv.PlayerDst.PlayerId,
                            Date = DateTime.UtcNow,
                            Type = evolib.PremadeGroup.ResponseType.Timeup,
                        })
                    );
                }
			}

			timeupInvs.ForEach(inv => RemoveInvitation(inv));
		}
	}
}
