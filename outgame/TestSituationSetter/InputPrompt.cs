using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter
{
	public static class InputPrompt
	{
		public static Task<string> Create()
		{
			return Task.Run<string>(() => {
				return Console.ReadLine();
			});
		}
	}
}
