using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter
{
	public static class ConsoleWriter
	{
		static void Print(string txt,
			ConsoleColor fore = ConsoleColor.White,
			ConsoleColor back=ConsoleColor.Black )
		{
			Console.ForegroundColor = fore;
			Console.BackgroundColor = back;

			Console.WriteLine(txt);

			Console.ResetColor();
		}

		public static void Info(string txt)
		{
			Print(txt, ConsoleColor.Yellow);
		}

		public static void Prompt(string txt)
		{
			Print(txt, ConsoleColor.Blue);
		}

		public static void Action(string txt)
		{
			Print(txt);
		}

		public static void Error(string txt)
		{
			Print(txt, ConsoleColor.Red);
		}
		public static void Warning(string txt)
		{
			Print(txt, ConsoleColor.DarkYellow);
		}
		public static void Succeeded(string txt)
		{
			Print(txt, ConsoleColor.Green);
		}
	}
}
