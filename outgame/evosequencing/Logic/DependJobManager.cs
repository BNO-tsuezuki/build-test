using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evosequencing.Logic
{
	public class DependJobManager
	{
		class Job
		{
			public bool Complete { get; set; }
			public Func<Task> DoAsync { get; set; }

			public long playerId1 { get; set; }
			public long playerId2 { get; set; }
		}

		object LockObj1 = new object();
		object LockObj2 = new object();
		List<Job> WaitingJobs { get; set; }

		public DependJobManager()
		{
			WaitingJobs = new List<Job>();

			Task.Run(async () =>
			{
				var dependPlayerIds = new List<long>();

				while (true)
				{
					lock (LockObj1)
					{
						for (int i = 0; i < WaitingJobs.Count;)
						{
							var job = WaitingJobs[i];

							if (dependPlayerIds.Contains(job.playerId1) || dependPlayerIds.Contains(job.playerId2))
							{
								i++;
								//Console.WriteLine($"depended:{job.playerId1}:{job.playerId2}");
								continue;
							}

							WaitingJobs.RemoveAt(i);
							lock (LockObj2)
							{
								dependPlayerIds.Add(job.playerId1);
								dependPlayerIds.Add(job.playerId2);
							}

							// No dependency!
							Task.Run(async () =>
							{
								try
								{
									await job.DoAsync();
								}
								catch (Exception)
								{
								}
								finally
								{
									job.Complete = true;

									lock (LockObj2)
									{
										dependPlayerIds.Remove(job.playerId1);
										dependPlayerIds.Remove(job.playerId2);
									}
								}
							});
						}
					}

					await Task.Delay(1);
				}
			});
		}

		public Task Enqueue(long playerId1, long playerId2, Func<Task> func)
		{
			var job = new Job()
			{
				DoAsync = func,
				Complete = false,
				playerId1 = playerId1,
				playerId2 = playerId2,
			};
			lock (LockObj1)
			{
				WaitingJobs.Add(job);
			}

			return Task.Run(async () =>
			{
				while (!job.Complete)
				{
					await Task.Delay(1);
				}
			});
		}
	}
}
