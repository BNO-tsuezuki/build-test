using System;

namespace evolib
{
	public class Permission : Util.Flags<Permission.Type>
    {
		public enum Type
		{
			CreateAccount=0,
			GrantPermission,
			MatchMake,
		}
	}
}
