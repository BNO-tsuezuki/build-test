using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;


using evomatching.Matching;

namespace MatchmakeTest
{
	class VirtualEntriesManager
	{
		class BattleEntryPlayer : IBattleEntryPlayer
		{
			public long PlayerId { get; set; }
			public string PlayerName { get; set; }
			public float Rating { get; set; }
			public string SessionId { get; set; }
		}

		class BattleEntry : IBattleEntry
		{
			public ulong EntryId { get; set; }

			public List<IBattleEntryPlayer> _Players { get; set; }
			public IReadOnlyList<IBattleEntryPlayer> Players
			{
				get { return _Players; }
			}

			public float EntryTime { get; set; }
			public TimeSpan WaitingTime { get { return TimeSpan.FromSeconds(Program.ElapsedSecond - EntryTime); } }


			public evolib.Battle.MatchType MatchType { get; set; }


			public float RatingSum { get; private set; }
			public float RatingAvg { get; private set; }
			public void RecalcRating()
			{
				if (Players.Count == 0)
				{
					RatingSum = 0;
					RatingAvg = 0;
					return;
				}

				float ratingSum = 0;
				for (int i = 0; i < Players.Count; i++)
				{
					var p = Players[i];
					ratingSum += p.Rating;
				}

				var coefficient = 1f;
				//coefficient += ((Players.Count - 1) * 0.05f);// TODO ★グループによる割り増し評価

				RatingSum = ratingSum * coefficient;
				RatingAvg = RatingSum / Players.Count;
			}

			public UInt64 MinPackageVersion { get; set; }
		}

		static int GroupSize(Random rnd)
		{
			var ratios = Program.GroupSizeRatio;

			var v = rnd.Next(0, ratios.Sum());

			var lim = 0;
			for (int j = 0; j < ratios.Length; j++)
			{
				lim += ratios[j];
				if (v < lim)
				{
					return j + 1;
				}
			}

			throw new Exception();
		}

		static ulong SerialEntryId { get; set; }

		Dictionary<ulong, IBattleEntry> Entries = new Dictionary<ulong, IBattleEntry>();

		public List<IBattleEntry> EntriesList { get { return Entries.Values.ToList(); } }

		public void Remove(ulong entryId)
		{
			if( Entries.ContainsKey(entryId))
			{
				EntryPlayerCount -= (Entries[entryId].Players.Count);
				Entries.Remove(entryId);
			}
		}

		public int EntryPlayerCount { get; private set; }

		public void AddEntry( int entryPlayerNum )
		{
			var rnd = new Random();

			while(EntryPlayerCount < entryPlayerNum)
			{
				var entry = new BattleEntry
				{
					EntryId = SerialEntryId++,
					_Players = new List<IBattleEntryPlayer>(),
					EntryTime = Program.ElapsedSecond,
					MatchType = evolib.Battle.MatchType.Casual,
				};

				var groupSize = GroupSize(rnd);
				var groupBaseRating = (float)evolib.Util.Gaussian.Next(Program.MinRating, Program.MaxRating);
				for (int j = 0; j < groupSize; j++)
				{
					entry._Players.Add(new BattleEntryPlayer
					{
						Rating = Math.Min(Program.MaxRating,
									Math.Max(Program.MinRating, groupBaseRating + rnd.Next(-500, 500))),
					});
				}
				entry.RecalcRating();
			
				Entries[entry.EntryId] = entry;
				EntryPlayerCount += groupSize;
			}
		}
	}
}
