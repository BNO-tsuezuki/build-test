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
	public enum OptNoticeType
	{
		Chat = 0,
		Popup,
        Topics,
	};

	public static class OpsNoticeManager
	{
		static Dictionary<string, OpsNotice> _Notices { get; set; }
		public static IReadOnlyDictionary<string, OpsNotice> Notices
		{
			get
			{
				return _Notices;
			}
		}
				

		static string NoticeCoded(OpsNotice rec)
		{
			return $"{rec.Id}_{rec.version}";
		}


		static public void Start()
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EVO_DBMIGRATION_FLAG")))
			{
				return;
			}

			_Notices = new Dictionary<string, OpsNotice>();

			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						using (var commonDB1 = DbContextFactory.CreateCommon1())
						{
							var dict = new Dictionary<string, OpsNotice>();
							var now = DateTime.UtcNow;
							var list = await commonDB1.OpsNotices.ToListAsync();

							list.ForEach(rec =>
							{
								if (!rec.release) return;

								if( (now < rec.beginDate || rec.endDate < now ))
								{
									return;
								}

								dict[NoticeCoded(rec)] = rec;
							});

							_Notices = dict;
						}
					}
					catch (Exception ex)
					{

					}

					await Task.Delay(10000);
				}
			});
		}
	}
}
