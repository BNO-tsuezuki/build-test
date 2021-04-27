using System;
using System.Threading.Tasks;


namespace TestSituationSetter
{
	class Program
	{
		static void Main(string[] args)
		{
			var mainStateMachine = State.MainStates.Main.Create();

			var mainTask = Task.Run(async () =>
			{
				while (true)
				{
					mainStateMachine.Update();

					//Console.WriteLine(DateTime.Now);
					await Task.Delay(100);
				}
			});

			//Console.ReadKey();
			Task.WaitAll(new Task[] { mainTask });
		}
	}
}
