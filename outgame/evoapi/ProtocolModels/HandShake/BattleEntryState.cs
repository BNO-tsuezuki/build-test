using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class BattleEntryState : HandShake
	{
		public class Response : ResponseBase
		{
			public evolib.Kvs.Models.ConnectionQueueData.BattleEntryState.State state { get; set; }
		}
	}
}
