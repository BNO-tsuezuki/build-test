using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text;

using evomatching.Matching;
using evomatching.Logic;

namespace MatchmakeTest
{
    class Program
    {
		static int TestTime { get { return 60 * 60 * 6; } }

		public static int ElapsedSecond { get; private set; }
		public static int[] GroupSizeRatio { get { return new int[] { 500, 250, 100, 40, 40, 70, }; } }
		public static float MinRating { get { return 500f;  } }
		public static float MaxRating { get { return 4000f; } }

		//public static int MinAu { get { return 10000; } }
		//public static int MaxAu { get { return 200000; } }
		//public static int MinAu { get { return 10; } }
		//public static int MaxAu { get { return 200; } }
		public static int MinAu { get { return 500; } }
		public static int MaxAu { get { return 3000; } }


		public static int WaitTimeRankMax {  get { return 500; } }


		public static int MatchMakeIntervalSec { get { return 30; } }

		class FileWriter
		{
			StreamWriter streamWriter;
			public int CurrentLineNo { get; private set; }

			public FileWriter(string fileName)
			{
				streamWriter = new StreamWriter($"{fileName}", false, new UTF8Encoding(true));
				CurrentLineNo = 1;
			}
			public void Close()
			{
				streamWriter.Close();
			}

			public void WriteLine(string text)
			{
				streamWriter.WriteLine(text);
				CurrentLineNo++;
			}
			public void Write(string text)
			{
				streamWriter.Write(text);
			}
		}


		static string DateFromSec(int second)
		{
			var hour = second / 3600;
			var min = (second % 3600) / 60;
			var sec = (second % 3600) % 60;
			return $"{hour:D2}:{min:D2}:{sec:D2}";
		}

		static void WriteEntryInfo(FileWriter writer, Matchmaker2.Match match)
		{
			var sideA = new List<IBattleEntry>();
			var sideB = new List<IBattleEntry>();
			foreach( var element in match.Elements)
			{
				if (element.Side == evolib.Battle.Side.Earthnoid) sideA.Add(element.Entry);
				if (element.Side == evolib.Battle.Side.Spacenoid) sideB.Add(element.Entry);
			}


			foreach (var ent in sideA)
			{
				for (int i = 0; i < ent.Players.Count; i++)
				{
					writer.Write($"{DateFromSec((int)ent.WaitingTime.TotalSeconds)},");
				}
			}
			foreach (var ent in sideB)
			{
				for (int i = 0; i < ent.Players.Count; i++)
				{
					writer.Write($"{DateFromSec((int)ent.WaitingTime.TotalSeconds)},");
				}
			}

			var partyNo = 1;
			foreach (var ent in sideA)
			{
				foreach (var p in ent.Players)
				{
					writer.Write($"{partyNo},");
				}
				partyNo++;
			}
			foreach (var ent in sideB)
			{
				foreach (var p in ent.Players)
				{
					writer.Write($"{partyNo},");
				}
				partyNo++;
			}

			var ratingSumA = 0f;
			var ratingSumB = 0f;
			foreach (var ent in sideA)
			{
				foreach (var p in ent.Players)
				{
					writer.Write($"{p.Rating:F0},");
				}

				ratingSumA += ent.RatingSum;
			}
			foreach (var ent in sideB)
			{
				foreach (var p in ent.Players)
				{
					writer.Write($"{p.Rating:F0},");
				}
				ratingSumB += ent.RatingSum;
			}
			
			writer.Write($"{ratingSumA},");
			writer.Write($"{ratingSumB},");
			writer.Write($"{MathF.Abs(ratingSumA-ratingSumB)},");
			writer.Write($"{MathF.Abs(ratingSumA - ratingSumB)/6},");
			writer.WriteLine("");
		}


		static void Main(string[] args)
		{
			var taskList = new List<Task>();
			taskList.Add(Start());

			Task.WaitAll(taskList.ToArray());
		}

		static Task Start()
		{
			return Task.Run(() =>
			{
				var startDate = DateTime.UtcNow;


				var fileMatchmakes	= new FileWriter(@"./matchmakes.csv");
				var fileDetails		= new FileWriter(@"./matchmakes_details.csv");


				fileMatchmakes.WriteLine($"#");
				fileMatchmakes.WriteLine($"{startDate}");
				fileMatchmakes.WriteLine($"マッチメイク情報");
				fileMatchmakes.WriteLine($"#");
				fileMatchmakes.WriteLine(",時刻,全人数,マッチ数,マッチ中人数,待機人数,メイク数,処理時間");

				fileDetails.WriteLine($"#");
				fileDetails.WriteLine($"{startDate}");
				fileDetails.WriteLine($"マッチメイク詳細");
				fileDetails.WriteLine($"#");
				fileDetails.WriteLine("Date,Wait,,,,,,,,,,,,PartyNo,,,,,,,,,,,,Rating,,,,,,,,,,,,A合計,B合計,合計差,差/人,");
				fileDetails.WriteLine("    ,A  ,,,,,,B,,,,,,A     ,,,,,,B,,,,,,A    ,,,,,,B,,,,,,");


				var matchManager = new VirtualMatchManager();
				var entriesManager = new VirtualEntriesManager();
				var matchMaker = new Matchmaker2();

				var waitTimeRanking = new WaitTimeRanking();

				var lastMatchmakeTime = ElapsedSecond;

				var sw = new System.Diagnostics.Stopwatch();
				while (true)
				{
					var  targetAuNum = MathF.Sin(MathF.PI * ElapsedSecond / TestTime) * (MaxAu - MinAu) + MinAu;

					entriesManager.AddEntry((int)targetAuNum - matchManager.MatchCount*12);

					if ((lastMatchmakeTime + MatchMakeIntervalSec) <= ElapsedSecond)
					{
						Console.WriteLine(DateFromSec(ElapsedSecond));
						fileMatchmakes.Write(",");
						fileMatchmakes.Write($"{DateFromSec(ElapsedSecond)},");
						fileMatchmakes.Write($"{matchManager.MatchCount * 12 + entriesManager.EntryPlayerCount},");
						fileMatchmakes.Write($"{matchManager.MatchCount},");
						fileMatchmakes.Write($"{matchManager.MatchCount * 12},");
						fileMatchmakes.Write($"{entriesManager.EntryPlayerCount},");

						sw.Restart();
						var matchmakeResult = matchMaker.MatchmakeForCBT1(MaxAu, entriesManager.EntriesList, 120);
						sw.Stop();

						fileMatchmakes.Write($"{matchmakeResult.Count},");
						fileMatchmakes.Write($"{sw.ElapsedMilliseconds},");
						fileMatchmakes.WriteLine("");

						matchManager.Matching(matchmakeResult.Count);

						foreach (var match in matchmakeResult)
						{
							foreach (var element in match.Elements)
							{
								entriesManager.Remove(element.Entry.EntryId);
								waitTimeRanking.Nominate(element.Entry, true);
							}

							fileDetails.Write($"{DateFromSec(ElapsedSecond)},");
							WriteEntryInfo(fileDetails, match);
						}

						lastMatchmakeTime = ElapsedSecond;
					}

					matchManager.CheckFinish();

					ElapsedSecond += 5;
					if (TestTime < ElapsedSecond)
					{
						break;
					}
				}

				entriesManager.EntriesList.ForEach(ent =>
				{
					waitTimeRanking.Nominate(ent, false);
				});



				var fileWaitingRank = new FileWriter(@"./matchmakes_waitingRank.csv");
				fileWaitingRank.WriteLine($"# 待ち時間ワースト {WaitTimeRankMax}");

				foreach ( var info in waitTimeRanking.RankingList)
				{
					fileWaitingRank.Write($"[{DateFromSec((int)info.WaitingTime.TotalSeconds)} {info.AlreadyMatchmake}],");

					foreach (var p in info.Ent.Players)
					{
						fileWaitingRank.Write($"{p.Rating:F0},");
					}
					fileWaitingRank.WriteLine("");
				}

				fileWaitingRank.Close();

				fileMatchmakes.Close();
				fileDetails.Close();
			});

		}
    }
}
