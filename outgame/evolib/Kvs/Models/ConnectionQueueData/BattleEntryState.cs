using System.Collections.Generic;

namespace evolib.Kvs.Models.ConnectionQueueData
{
    public class BattleEntryState : ConnectionQueue.Data
	{
		public enum State
		{
			CasualMatchEntry = 0,
			RankMatchEntry,
			Cancel,
		}

		public State state  { get; set; }
	}
}
