using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using evolib.Log;


namespace evomatching.Matching
{
	public partial class PremadeGroupManager
	{
		class PremadeGroupPlayer : IPremadeGroupPlayer
		{
			public long PlayerId { get; set; }
			public string SessionId { get; set; }
		}

		class PremadeGroup : IPremadeGroup
		{
			public string GroupId { get; set; }

			public List<IPremadeGroupPlayer> _Players;
			public IReadOnlyList<IPremadeGroupPlayer> Players{ get { return _Players; } }

			public long LeaderPlayerId { get; set; }
		}

		Dictionary<string, PremadeGroup> Groups = new Dictionary<string, PremadeGroup>();

		Dictionary<long, PremadeGroup> BelongsMap = new Dictionary<long, PremadeGroup>();


		public IPremadeGroup GetBelongs(long playerId)
		{
			if (BelongsMap.ContainsKey(playerId))
			{
				return BelongsMap[playerId];
			}

			return null;
		}

		public bool Form(IPgInvitation inv)
		{
			if (BelongsMap.ContainsKey(inv.PlayerDst.PlayerId))
			{
				return false;
			}

			PremadeGroup group;

			if (BelongsMap.ContainsKey(inv.PlayerSrc.PlayerId))
			{
				group = BelongsMap[inv.PlayerSrc.PlayerId];

				group._Players.Add(inv.PlayerDst);

				BelongsMap[inv.PlayerDst.PlayerId] = group;
			}
			else
			{
				group = new PremadeGroup()
				{
					GroupId = evolib.Util.KeyGen.Get(32),
					LeaderPlayerId = inv.PlayerSrc.PlayerId,
					_Players = new List<IPremadeGroupPlayer>() { inv.PlayerSrc, inv.PlayerDst },
				};

				Groups[group.GroupId] = group;

				BelongsMap[inv.PlayerSrc.PlayerId] = group;
				BelongsMap[inv.PlayerDst.PlayerId] = group;
			}

			return true;
		}


		public List<IPremadeGroupPlayer> Leave(long playerId)
		{
			var ret = new List<IPremadeGroupPlayer>();

			if (!BelongsMap.ContainsKey(playerId)) return ret;

			var group = BelongsMap[playerId];
			group._Players.ForEach(p => ret.Add(p));

			var idx = group._Players.FindIndex(p => p.PlayerId == playerId);
			group._Players.RemoveAt(idx);

			BelongsMap.Remove(playerId);

			if (group.Players.Count == 1)
			{
				Groups.Remove(group.GroupId);

				BelongsMap.Remove(group.Players[0].PlayerId);
			}
			else
			{
				if (group.LeaderPlayerId == playerId)
				{
					group.LeaderPlayerId = group.Players[0].PlayerId;
				}
			}

			return ret;
		}

		public List<IPremadeGroupPlayer> Kick(long playerIdSrc, long playerIdDst)
		{
			var ret = new List<IPremadeGroupPlayer>();

			if (playerIdSrc == playerIdDst) return ret;
			if (!BelongsMap.ContainsKey(playerIdSrc)) return ret;
			if (!BelongsMap.ContainsKey(playerIdDst)) return ret;

			var groupSrc = BelongsMap[playerIdSrc];
			var groupDst = BelongsMap[playerIdDst];

			if (groupSrc.GroupId != groupDst.GroupId) return ret;

			var group = groupSrc;

			if (group.LeaderPlayerId != playerIdSrc) return ret;

			return Leave(playerIdDst);
		}

		public List<IPremadeGroupPlayer> TransferLeader(long playerIdSrc, long playerIdDst)
		{
			var ret = new List<IPremadeGroupPlayer>();

			if (playerIdSrc == playerIdDst) return ret;
			if (!BelongsMap.ContainsKey(playerIdSrc)) return ret;
			if (!BelongsMap.ContainsKey(playerIdDst)) return ret;

			var groupSrc = BelongsMap[playerIdSrc];
			var groupDst = BelongsMap[playerIdDst];

			if (groupSrc.GroupId != groupDst.GroupId) return ret;

			var group = groupSrc;

			if (group.LeaderPlayerId != playerIdSrc) return ret;

			group.LeaderPlayerId = playerIdDst;

			group._Players.ForEach(p => ret.Add(p));
			return ret;
		}


		
		public List<IPremadeGroupPlayer> CheckSessions(Func<string, bool> aliveChecker)
		{
			var delPlayerIdList = new List<long>();

			foreach ( var group in Groups.Values )
			{
				group._Players.ForEach(p =>
				{
					if (!aliveChecker(p.SessionId))
					{
						delPlayerIdList.Add(p.PlayerId);

                        List<long> playerIds = new List<long>();
                        for (int i = 0; i < group._Players.Count; i++)
                        {
                            var member = group._Players[i];
                            if (member.PlayerId == p.PlayerId) continue;
                            playerIds.Add(member.PlayerId);
                        }
                        Logger.Logging(
                            new LogObj().AddChild(new LogModels.UpdateParty
                            {
                                GroupId = group.GroupId,
                                PlayerId = p.PlayerId,
                                Date = DateTime.UtcNow,
                                Type = evolib.PremadeGroup.Type.SessionError,
                                PlayerIds = playerIds,
                            })
                        );
                    }
                });
			}

			var changedPlayers = new Dictionary<long, IPremadeGroupPlayer>();

			delPlayerIdList.ForEach(pid =>
			{
				Leave(pid).ForEach(player => changedPlayers[player.PlayerId] = player);
				changedPlayers.Remove(pid);
			});

			return changedPlayers.Values.ToList();
		}
	}
}
