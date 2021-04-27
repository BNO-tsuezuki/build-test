using System;

namespace evolib.Util
{
    public abstract class Flags<T> where T:Enum
    {
		public int Value { get; set; }

		public bool Check(T type)
		{
			return (0 != (Value & (1 << Convert.ToInt32(type))));
		}

		public void Give(Type type)
		{
			Value |= (1 << Convert.ToInt32(type));
		}

		public void Take(Type type)
		{
			Value &= ~(1 << Convert.ToInt32(type));
		}
	}
}
