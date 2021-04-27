using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter
{
	public static class InputPassPrompt
	{
		public static Task<string> Create()
		{
			return Task.Run<string>(() =>
			{
				var pass = "";
				while (true)
				{
					var info = Console.ReadKey(true);

					if (info.Key == ConsoleKey.Enter)
					{
						Console.Write("\n");
						break;
					}

					if (info.Key == ConsoleKey.Backspace)
					{
						if (0 < pass.Length)
						{
							pass.Remove(pass.Length - 1);
							Console.Write("\b \b");
						}
					}
					else if (info.KeyChar != '\u0000')
					{
						pass += info.KeyChar;
						Console.Write("*");
					}
				}

				return pass;
			});
		}
	}
}
