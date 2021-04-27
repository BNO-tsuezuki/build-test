using System;
using System.Collections.Generic;
using System.Text;

namespace evolib.Util
{
    public static class Gaussian
    {
		static Random Rand = new Random();
		public static double Next(double min, double max)
		{
			var n = 0.0;
			do
			{
				n = Math.Sqrt(-2.0 * Math.Log(Rand.NextDouble())) * Math.Sin(2.0 * Math.PI * Rand.NextDouble());

			} while (3.0 < Math.Abs(n));

			var range = max - min;
			var halfRange = range / 2;
			var avg = min + halfRange;

			return avg + halfRange * n / 3.0;
		}
    }
}
