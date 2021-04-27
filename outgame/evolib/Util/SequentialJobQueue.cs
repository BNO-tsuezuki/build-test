using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Util
{
	public class SequentialJobQueue
	{
		class Job
		{
			public bool Complete { get; set; }
			public Func<Task> DoAsync { get; set; }
			public Action Do { get; set; }
			public bool isAsync { get; set; }
		}

		object LockObj = new object();
		Queue<Job> JobQueue { get; set; }

		public SequentialJobQueue()
		{
			JobQueue = new Queue<Job>();

			Task.Run(async () =>
			{
				while (true)
				{
					await Processing();

					await Task.Delay(1);
				}
			});
		}

		public Task Enqueue(Func<Task> func)
		{
			var job = new Job();
			job.DoAsync = func;
			job.isAsync = true;
			job.Complete = false;
			lock (LockObj)
			{
				JobQueue.Enqueue(job);
			}

			return Task.Run( async () =>
			{
				while (!job.Complete)
				{
					await Task.Delay(1);
				}
			});
		}

		public Task Enqueue(Action func)
		{
			var job = new Job();
			job.Do = func;
			job.isAsync = false;
			job.Complete = false;
			lock (LockObj)
			{
				JobQueue.Enqueue(job);
			}

			return Task.Run(async () =>
			{
				while (!job.Complete)
				{
					await Task.Delay(1);
				}
			});
		}

		async Task Processing()
		{
			while (true)
			{
				Job job = null;
				lock (LockObj)
				{
					if (JobQueue.Count == 0) break;
					job = JobQueue.Dequeue();
				}

				try
				{
					if (job.isAsync)
					{
						await job.DoAsync();
					}
					else
					{
						job.Do();
					}
				}
				catch(Exception)
				{ }
				finally
				{
					job.Complete = true;
				}
			}
		}
	}
}
