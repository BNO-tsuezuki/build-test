using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
	public interface IVoicePack
	{
		string itemId { get; }
		bool IsIncludeVoice(string voiceId);
		string DefaultVoiceId { get;  }
		string EmptyVoiceId { get; }
	}

	public class VoicePack : IVoicePack
    {
		public string itemId { get; private set; }

		public bool IsIncludeVoice(string voiceId)
		{
			return voices.ContainsKey(voiceId);
		}

		public string DefaultVoiceId { get; private set; }

		public string EmptyVoiceId { get; private set; }

		//----
		public VoicePack(string voicePackItemId)
		{
			itemId = voicePackItemId;
			DefaultVoiceId = "";
			EmptyVoiceId = "";
		}

		public void addVoice( string id, bool isDefault, bool isEmpty )
		{
			voices[id] = 1234;
			if( isDefault )
			{
				DefaultVoiceId = id;
			}
			if( isEmpty )
			{
				EmptyVoiceId = id;
			}
		}

		Dictionary<string, int> voices = new Dictionary<string, int>();
	}
}
