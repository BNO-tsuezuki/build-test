using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.OwnMobileSuitSetting
{
    public class SetWeaponSkin
	{
		public class Request
		{
			[Required]
			public string mobileSuitId { get; set; }

			[Required]
			public string weaponSkinItemId { get; set; }
		}


		public class Response
		{
			public Setting setting { get; set; }
		}
	}
}
