using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using evolib.Databases;
using evolib.Databases.common2;

namespace evolib
{
    public static class DisabledMobileSuit
    {
		static List<string> DisabledMobileSuits;

		static public void Start()
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EVO_DBMIGRATION_FLAG")))
			{
				return;
			}

			DisabledMobileSuits = new List<string>();

			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						var list = new List<string>();

						using (var commonDB2 = DbContextFactory.CreateCommon2())
						{
							var all = await commonDB2.DisabledMobileSuits.ToListAsync();

							foreach (var r in all)
							{
								list.Add(r.itemId);
							}
						}

						DisabledMobileSuits = list;
					}
					catch (Exception ex)
					{
					}

					await Task.Delay(10000);
				}
			});
		}

		public static List<string> DisabledThings()
		{
			var list = DisabledMobileSuits;
			return list.ToList();
		}
	}
}
