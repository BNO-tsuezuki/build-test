using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public interface IPgInvitation
	{
		IPremadeGroupPlayer PlayerSrc { get; }
		IPremadeGroupPlayer PlayerDst { get; }

		DateTime Expiry { get; }
	}
}
