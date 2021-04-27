using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Simulator : MonoBehaviour {

	[SerializeField]
	Transform Origin;

	[SerializeField]
	GameObject prefabGraphBar;

	[SerializeField]
	Text BattleCnt;


	class Player
	{
		public float 実力レート値;
		public float 計算レート値;
		public int 勝ち数;
		public int 負け数;
		public float 勝率合計;
	}

	float DistributionRange = 1500;
	float Median = 2000;
	float PlayerNum = 12000;

	float BaseValue = 25f;
	float TeamCorrectionFactor = 0.8f;
	float MaxRateDiff = 200;


	List<Player> Players = new List<Player>();

	float Gaussian()
	{//box-muller
		return Mathf.Sqrt(-2f * Mathf.Log(Random.value)) * Mathf.Sin(2f * Mathf.PI * Random.value);
	}

	class Range
	{
		public float min;
		public float max;
		public GraphBar bar1;
		public GraphBar bar2;
	}


	// Use this for initialization
	IEnumerator Start ()
	{
		for (int i = 0; i < PlayerNum; i++)
		{
			var v = 0f;
			do
			{
				v = Gaussian();

			} while (3 < Mathf.Abs(v));

			var newPlayer = new Player()
			{
				実力レート値 = Median - DistributionRange * v / 3,
				計算レート値 = Median,
				勝ち数 = 0,
				負け数 = 0,
			};
			Players.Add(newPlayer);
		}

		var ranges = new List<Range>();
		var pos = new Vector2() { x = 160, y = -20 };
		var min = float.MinValue;
		var max = -1000f;
		while( true )
		{
			var r = new Range()
			{
				bar1 = Instantiate(prefabGraphBar, Origin).GetComponent<GraphBar>(),
				bar2 = Instantiate(prefabGraphBar, Origin).GetComponent<GraphBar>(),
				min = min,
				max = max,
			};
			r.bar1.transform.localPosition = pos;
			r.bar2.transform.localPosition = pos + Vector2.down * 10;
			r.bar1.Caption = "" + max.ToString("F0");
			r.bar2.Caption = "";
			r.bar1.Color = Color.magenta;
			r.bar2.Color = Color.yellow;
			ranges.Add(r);

			pos += Vector2.down * 32;
			min = max;
			max = min + 100;
			if (5000 < max) break;
		}

		ranges.ForEach(rng =>
		{
			int cnt1 = 0;
			int cnt2 = 0;
			Players.ForEach(p =>
			{
			   if (rng.min <= p.実力レート値 && p.実力レート値 < rng.max)
			   {
				   cnt1++;
			   }
				if (rng.min <= p.計算レート値 && p.計算レート値 < rng.max)
				{
					cnt2++;
				}
			});
			rng.bar1.Value = cnt1;
			rng.bar2.Value = cnt2;
		});


		for (int i=0; i<600; i++ )
		{
			Matchmake();

			ranges.ForEach(rng =>
			{
				int cnt2 = 0;
				Players.ForEach(p =>
				{
					if (rng.min <= p.計算レート値 && p.計算レート値 < rng.max)
					{
						cnt2++;
					}
				});
				rng.bar2.Value = cnt2;
				rng.bar2.Caption = "[" + cnt2 + "]                ";
			});

			BattleCnt.text = (i + 1).ToString();
			
			yield return 1;
		}

		var str = "期待勝ち数,勝ち数,負け数,勝率,実力,計算,誤差,勝ち数誤差\n";
		var sum絶対差 = 0f;
		var sum差の二乗 = 0f;
		var sum勝ち数誤差 = 0f;
		Players.ForEach(p =>
		{
			var 差 = p.計算レート値 - p.実力レート値;
			var 差の二乗 = 差 * 差;
			var 勝ち数誤差 = p.勝率合計 - p.勝ち数;

			str += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\n",
			p.勝率合計,
			p.勝ち数,
			p.負け数,
			(float)p.勝ち数 / (p.勝ち数 + p.負け数),
			p.実力レート値,
			p.計算レート値,
			差,
			勝ち数誤差
			);

			sum差の二乗 += 差の二乗;
			sum絶対差 += Mathf.Abs(差);
			sum勝ち数誤差 += Mathf.Abs(勝ち数誤差);
		});
		System.IO.File.WriteAllText("C:\\Users\\t-yoshimura\\Desktop\\aaaaa.csv",
			//"分散," + sum差の二乗/Players.Count + "\n" +
			//"偏差," + Mathf.Sqrt(sum差の二乗 / Players.Count) + "\n" +
			"誤差の平均," + sum絶対差 / Players.Count + "\n" +
			"勝ち数誤差の平均," + sum勝ち数誤差 / Players.Count + "\n" + 
			str );

		yield break;
	}

	


	void Matchmake()
	{
		var sortedPlayers = new List<Player>();
		Players.ForEach(p =>
		{
			sortedPlayers.Add(p);
		});
		sortedPlayers.Sort((a, b) => { return (int)(a.計算レート値 - b.計算レート値); });


		while (true)
		{
			if (sortedPlayers.Count == 0) break;

			var groupNum = Mathf.Max(12, (int)(PlayerNum * 0.01f));
			var matchingGroup = new List<Player>();
			while (true)
			{
				if (sortedPlayers.Count == 0) break;

				matchingGroup.Add(sortedPlayers[0]);
				sortedPlayers.RemoveAt(0);

				if (matchingGroup.Count == groupNum) break;
			}

			while (true)
			{
				if (matchingGroup.Count < 12) break;

				var teamAteamB = new List<Player>();
				for (int i = 0; i < 12; i++)
				{
					var idx = Random.Range(0, matchingGroup.Count);
					teamAteamB.Add(matchingGroup[idx]);
					matchingGroup.RemoveAt(idx);
				}

				Battle(teamAteamB);
			}
		}
	}

	void Battle(List<Player> players )
	{
		var teamA = new List<Player>() { players[0], players[1], players[2], players[3], players[4], players[5], };
		var teamB = new List<Player>() { players[6], players[7], players[8], players[9], players[10], players[11], };

		var 実力A = 0f;
		var 計算A = 0f;
		teamA.ForEach(p => { 実力A += p.実力レート値; 計算A += p.計算レート値; });
		実力A /= teamA.Count;
		計算A /= teamA.Count;

		var 実力B = 0f;
		var 計算B = 0f;
		teamB.ForEach(p => { 実力B += p.実力レート値; 計算B += p.計算レート値; });
		実力B /= teamB.Count;
		計算B /= teamB.Count;

		var 実力差A = 実力B - 実力A;
		var 計算差A = 計算B - 計算A;
		計算差A = Mathf.Min(Mathf.Abs(計算差A), MaxRateDiff) * Mathf.Sign(計算差A);

		var 実力WinPercentageA = 1f / (Mathf.Pow(10, 実力差A / 400f) + 1f);
		var 計算WinPercentageA = 1f / (Mathf.Pow(10, 計算差A / 400f) + 1f);

		if ( Random.value < 実力WinPercentageA)
		{// win A team
			var dr = BaseValue + BaseValue * (0.5f - 計算WinPercentageA) * TeamCorrectionFactor;
			TeamAdjustment(teamA, 計算A, dr, true, 実力WinPercentageA );
			TeamAdjustment(teamB, 計算B, dr, false, 1f - 実力WinPercentageA);
			//Debug.Log(winPercentage + "@" + dr);
		}
		else
		{// win B team
			var dr = BaseValue + BaseValue * (計算WinPercentageA - 0.5f) * TeamCorrectionFactor;
			TeamAdjustment(teamA, 計算A, dr, false, 実力WinPercentageA);
			TeamAdjustment(teamB, 計算B, dr, true, 1f- 実力WinPercentageA);
			//Debug.Log(1-winPercentage + "@" + dr);
		}
	}

	void TeamAdjustment(List<Player> team, float averageRate, float gain, bool win, float 勝率)
	{

		team.ForEach(p =>
		{
			var personalGain = (gain + (win ? 1f : -1f) * gain * (averageRate - p.計算レート値) / averageRate)
								* (win ? 1f : -1f);


			p.計算レート値 += personalGain;
			p.勝率合計 += 勝率;

			if (win)
			{
				p.勝ち数++;
			}
			else
			{
				p.負け数++;
			}
		});
	}

}
