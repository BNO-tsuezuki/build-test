using System;
using System.Collections.Generic;


namespace evolib.Services.MasterData
{
	public interface IAssetsInfo
	{
		string assetsId { get; }

		Int64 maxValue { get; }

		string type { get; }
	}

    public class AssetsInfo : IAssetsInfo
	{
		public string assetsId { get; set; }

		public Int64 maxValue { get; set; }

		public string type { get; set; }
	}
}
