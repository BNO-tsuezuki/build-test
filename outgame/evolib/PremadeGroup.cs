using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
	public static class PremadeGroup
	{
		public static TimeSpan InvitationExpiry = TimeSpan.FromSeconds(10);

        public enum Type
        {
            Entry = 0,
            Leave,
            Kick,
            SessionError,
        }

        public enum ResponseType
        {
            Join = 0,
            Deny,
            Timeup,
        }
    }
}
