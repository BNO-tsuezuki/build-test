using System;

namespace evolib.Kvs
{
    public class KvsModelAttribute : Attribute
    {
		public KvsType kvsType { get; set; }
	}
}
