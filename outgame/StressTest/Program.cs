using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
			//System.Threading.Thread.Sleep(5000);

			var taskList = new List<Task>();

			for (int i = 0; i < 10; i++)
			{
				var player = new Player();
				taskList.Add(player.Start(i));

				System.Threading.Thread.Sleep(10);
			}

			Task.WaitAll( taskList.ToArray());
        }
    }
}
