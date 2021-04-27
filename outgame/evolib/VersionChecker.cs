using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using evolib.Databases;
using evolib.Databases.common1;

namespace evolib
{
    public static class VersionChecker
    {
		public enum CheckTarget
		{
			Login = 0,
			Matchmake,
			Replay,



			EnabledMatchmake = 1000000,
		}

		public enum ReferenceSrc
		{
			PackageVersion = 0,
			MasterDataVersion,
		}

		public enum Const
		{
			PackageVersionLength = 255,
		}

		public class CheckItem
		{
			public CheckTarget checkTarget { get; set; }

			public ReferenceSrc referenceSrc { get; set; }

			//public Version version { get; set; }

			public int[] array { get; set; }
			public UInt64 value { get; set; }
		}

		static List<CheckItem> CheckItems;

		public static UInt64 Valued( ReferenceSrc refSrc, int[] version)
		{
			var dst = new int[] { 0, 0, 0, 0 };
			if( version != null)
			{
				if (0 < version.Length) dst[0] = Math.Max(version[0], 0);
				if (1 < version.Length) dst[1] = Math.Max(version[1], 0);
				if (2 < version.Length) dst[2] = Math.Max(version[2], 0);
				if (3 < version.Length) dst[3] = Math.Max(version[3], 0);
			}

			var str = "0";
			switch( refSrc )
			{
				case ReferenceSrc.PackageVersion:
					dst[0] = Math.Min(dst[0], 9);
					dst[1] = Math.Min(dst[1], 99);
					dst[2] = Math.Min(dst[2], 99);
					dst[3] = Math.Min(dst[3], 999999);
					str = $"{dst[0]:D1}{dst[1]:D02}{dst[2]:D02}{dst[3]:D06}";
					break;
				case ReferenceSrc.MasterDataVersion:
					dst[0] = Math.Min(dst[0], 999);
					dst[1] = Math.Min(dst[1], 999999);
					dst[2] = Math.Min(dst[2], 999);
					str = $"{dst[0]:D03}{dst[1]:D06}{dst[2]:D03}";
					break;
			}

			return UInt64.Parse(str);
		}

		static public void Start()
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EVO_DBMIGRATION_FLAG")))
			{
				return;
			}

			CheckItems = new List<CheckItem>();

			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						var list = new List<CheckItem>();

						using (var commonDB1 = DbContextFactory.CreateCommon1())
						{
							var all = await commonDB1.EnabledVersions.ToListAsync();

							foreach (var r in all)
							{
								list.Add(new CheckItem
								{
									checkTarget = r.checkTarget,
									referenceSrc = r.referenceSrc,
									//version = new Version( r.major, r.minor, r.patch, r.build ),
									array = new int[] { r.major, r.minor, r.patch, r.build },
									value = Valued(r.referenceSrc, new int[] { r.major, r.minor, r.patch, r.build }),
								});
							}
						}

						CheckItems = list;
					}
					catch (Exception ex)
					{

					}

					await Task.Delay(10000);
				}
			});
		}

		public class Checker
		{
			List<CheckItem> CheckItems;
			public CheckTarget CheckTarget { get; private set; }
		
			public Checker(CheckTarget checkTarget, List<CheckItem> checkItems)
			{
				CheckTarget = checkTarget;
				CheckItems = checkItems;
			}

			public UInt64 PackageVersion { get; set; }
			public UInt64 MasterDataVersion { get; set; }
			public bool Check()
			{
				foreach( var item in CheckItems)
				{
					if (item.checkTarget != CheckTarget) continue;

					switch (item.referenceSrc)
					{
						case ReferenceSrc.PackageVersion:
							if (PackageVersion < item.value) return false;
							break;
						case ReferenceSrc.MasterDataVersion:
							if (MasterDataVersion < item.value) return false;
							break;
					}
				}

				return true;
			}
		}

		public static Checker Get( CheckTarget target )
		{
			return new Checker(target, CheckItems);
		}

		public static int[] LimitPackageVersion(CheckTarget target)
		{
			var checkItems = CheckItems;

			foreach (var item in checkItems)
			{
				if (item.checkTarget == target && item.referenceSrc == ReferenceSrc.PackageVersion)
				{
					return item.array;
				}
			}

			return new int[] { 0, 0, 0, 0 };
		}
	}
}
