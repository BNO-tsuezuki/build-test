using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace evomatching.Logic
{
	public class Matchmaker2
	{
		static int MaxCandidatesNum = 400;
		static float LimitRatingRange = 500;

		static bool CheckForCandidates(Matching.IBattleEntry current, Matching.IBattleEntry target)
		{
			if (LimitRatingRange <= MathF.Abs(target.RatingAvg - current.RatingAvg))
			{
				return false;
			}

			return true;
		}

		public static Func<int, float> Func_DiffGroupSize =
		(diff) =>
		{
			return diff * 40f;
		};

		public static Func<float, float> Func_DiffMatchRatingAvg =
		(diff) =>
		{
			return diff * 1f;
		};

		public static Func<float, float> Func_DiffMatchPingAvg =
		(diff) =>
		{
			return diff * 3f;
		};

		public static Func<float, float> Func_ElapsedTimeValue =
		(elapsedSec) =>
		{
			return Math.Min(elapsedSec * 0.22f, 250f);
		};



		public class Match
		{
			public class Element
			{
				public evolib.Battle.Side Side { get; set; }
				public Matching.IBattleEntry Entry { get; set; }
			}

			public List<Element> Elements { get; set; }
		}

		class Side
		{
			List<Matching.IBattleEntry> Entries = new List<Matching.IBattleEntry>();

			public void ForEach(Action<Matching.IBattleEntry> action)
			{
				Entries.ForEach(e => action(e));
			}
			public Side()
			{
				MaxGroupSize = 1;
			}

			public int PlayersCount { get; private set; }

			public int MaxGroupSize { get; private set; }

			public float RatingSum { get; private set; }

			public void Add(Matching.IBattleEntry entry)
			{
				Entries.Add(entry);

				var groupSize = entry.Players.Count;

				PlayersCount += groupSize;
				if( MaxGroupSize < groupSize)
				{
					MaxGroupSize = groupSize;
				}

				RatingSum += entry.RatingSum;
			}

			public bool CanAdd(Matching.IBattleEntry entry)
			{
				if(evolib.Battle.SidePlayersNum < (PlayersCount + entry.Players.Count))
				{
					return false;
				}

				return true;
			}
		}

		class BestEntry
		{
			public Matching.IBattleEntry entry { get; set; }
			public int index { get; set; }
			public float score { get; set; }
		}


		public List<Match> Matchmake(
			int matchmakeNum,
			List<Matching.IBattleEntry> entries,
			int minMatchmakeEntriedPlayersNumber

		){
			var ret = new List<Match>();

			// count entriedPlayers
			var entriedPlayersCnt = 0;
			foreach( var entry in entries)
			{
				entriedPlayersCnt += entry.Players.Count;
			}

			if(entriedPlayersCnt < minMatchmakeEntriedPlayersNumber)
			{
				return ret;
			}

			var subEntries = new List<Matching.IBattleEntry>();

			while (true)
			{
				if( matchmakeNum <= ret.Count)
				{
					break;
				}

				if (entries.Count <= 0)
				{
					break;
				}

				// Current entry.
				var currentEntry = entries[0];
				entries.RemoveAt(0);

				// Create candidates list.
				var candidatesList = new List<Matching.IBattleEntry>();
				foreach (var entry in entries)
				{
					if( MaxCandidatesNum <= candidatesList.Count)
					{
						break;
					}

					if(CheckForCandidates(currentEntry, entry))
					{
						candidatesList.Add(entry);
					}
				}
				foreach( var entry in subEntries)
				{
					if (MaxCandidatesNum <= candidatesList.Count)
					{
						break;
					}

					if (CheckForCandidates(currentEntry, entry))
					{
						candidatesList.Add(entry);
					}
				}

				var removeEntries = new Dictionary<ulong, int>();

				var sideA = new Side();
				var sideB = new Side();

				sideA.Add(currentEntry);

				while (true)
				{
					var bestEntry = new BestEntry();
					bestEntry.score = float.MinValue;

					var currentSide = (sideA.PlayersCount < sideB.PlayersCount) ? sideA : sideB;
					var opponetSide = (currentSide == sideA) ? sideB : sideA;

					for ( int i=0; i<candidatesList.Count; i++)
					{
						var targetEntry = candidatesList[i];

						if( !currentSide.CanAdd(targetEntry) )
						{
							continue;
						}

						var distance = 0f;

						//
						distance += MathF.Pow(
							Func_DiffGroupSize(opponetSide.MaxGroupSize - targetEntry.Players.Count),
							2
						);

						//
						distance += MathF.Pow(
							Func_DiffMatchRatingAvg(
								(currentSide.RatingSum + targetEntry.RatingSum) / (currentSide.PlayersCount + targetEntry.Players.Count)
										- opponetSide.RatingSum / opponetSide.PlayersCount ),
							2
						);

						//
						//distance += MathF.Pow(
						//	Func_DiffMatchPingAvg(matchPingAvg - targetEntry.PingAvg),
						//	2);


						//
						distance -= MathF.Pow(
							Func_ElapsedTimeValue((float)targetEntry.WaitingTime.TotalSeconds),
							2
						);

						var score = -distance;
						if (bestEntry.score < score)
						{
							bestEntry.entry = targetEntry;
							bestEntry.index = i;
							bestEntry.score = score;
						}
					}

					if( bestEntry.entry == null)
					{
						break;
					}

					currentSide.Add(bestEntry.entry);
					candidatesList.RemoveAt(bestEntry.index);

					if (evolib.Battle.MatchPlayersNum <= (sideA.PlayersCount + sideB.PlayersCount))
					{ // establishment
						var m = new Match
						{
							Elements = new List<Match.Element>(),
						};
						sideA.ForEach(e =>
						{
							m.Elements.Add(new Match.Element
							{
								Side = evolib.Battle.Side.Earthnoid,
								Entry = e,
							});
							removeEntries[e.EntryId] = 0;
						});
						sideB.ForEach(e =>
						{
							m.Elements.Add(new Match.Element
							{
								Side = evolib.Battle.Side.Spacenoid,
								Entry = e,
							});
							removeEntries[e.EntryId] = 0;
						});

						ret.Add(m);

						break;
					}
				}

				for ( int i=0; i<entries.Count; )
				{
					if( removeEntries.ContainsKey(entries[i].EntryId))
					{
						entries.RemoveAt(i);
						continue;
					}
					i++;
				}
				for( int i=0; i<subEntries.Count;)
				{
					if( removeEntries.ContainsKey(subEntries[i].EntryId))
					{
						subEntries.RemoveAt(i);
						continue;
					}
					i++;
				}

				if (!removeEntries.ContainsKey(currentEntry.EntryId))
				{//because match not establishment
					subEntries.Add(currentEntry);
				}
			}

			return ret;
		}






















		public List<Match> MatchmakeForCBT1(
			int matchmakeNum,
			List<Matching.IBattleEntry> entries,
			int minMatchmakeEntriedPlayersNumber

		){
			var ret = new List<Match>();

			// count entriedPlayers
			var entriedPlayersCnt = 0;
			foreach (var entry in entries)
			{
				entriedPlayersCnt += entry.Players.Count;
			}

			if (entriedPlayersCnt < minMatchmakeEntriedPlayersNumber)
			{
				return ret;
			}

			var subEntries = new List<Matching.IBattleEntry>();

			while (true)
			{
				if (matchmakeNum <= ret.Count)
				{
					break;
				}

				if (entries.Count <= 0)
				{
					break;
				}

				// Current entry.
				var currentEntry = entries[0];
				entries.RemoveAt(0);

				// Create candidates list.
				var rand = new Random();
				var candidatesList = new List<Matching.IBattleEntry>();
				foreach (var entry in subEntries)
				{
					if (MaxCandidatesNum <= candidatesList.Count)
					{
						break;
					}

					candidatesList.Add(entry);
				}
				foreach (var entry in entries)
				{
					if (MaxCandidatesNum <= candidatesList.Count)
					{
						break;
					}

					var idx = rand.Next(candidatesList.Count+1);
					candidatesList.Insert(idx, entry);
				}

				var removeEntries = new Dictionary<ulong, int>();

				var sideA = new Side();
				var sideB = new Side();

				sideA.Add(currentEntry);

				while (true)
				{
					var currentSide = (sideA.PlayersCount < sideB.PlayersCount) ? sideA : sideB;
					var opponetSide = (currentSide == sideA) ? sideB : sideA;

					var hit = false;
					for (int i = 0; i < candidatesList.Count; i++)
					{
						var targetEntry = candidatesList[i];

						if (!currentSide.CanAdd(targetEntry))
						{
							continue;
						}

						currentSide.Add(targetEntry);
						candidatesList.RemoveAt(i);
						hit = true;
						break;
					}

					if (!hit)
					{
						break;
					}

					if (evolib.Battle.MatchPlayersNum <= (sideA.PlayersCount + sideB.PlayersCount))
					{ // establishment
						var m = new Match
						{
							Elements = new List<Match.Element>(),
						};
						sideA.ForEach(e =>
						{
							m.Elements.Add(new Match.Element
							{
								Side = evolib.Battle.Side.Earthnoid,
								Entry = e,
							});
							removeEntries[e.EntryId] = 0;
						});
						sideB.ForEach(e =>
						{
							m.Elements.Add(new Match.Element
							{
								Side = evolib.Battle.Side.Spacenoid,
								Entry = e,
							});
							removeEntries[e.EntryId] = 0;
						});

						ret.Add(m);

						break;
					}
				}

				for (int i = 0; i < entries.Count;)
				{
					if (removeEntries.ContainsKey(entries[i].EntryId))
					{
						entries.RemoveAt(i);
						continue;
					}
					i++;
				}
				for (int i = 0; i < subEntries.Count;)
				{
					if (removeEntries.ContainsKey(subEntries[i].EntryId))
					{
						subEntries.RemoveAt(i);
						continue;
					}
					i++;
				}

				if (!removeEntries.ContainsKey(currentEntry.EntryId))
				{//because match not establishment
					subEntries.Add(currentEntry);
				}
			}

			return ret;
		}
	}
}
