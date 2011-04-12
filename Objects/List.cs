using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	public class List : Value
	{
		public List(params Value[] Values)
			: this(Values.ToList()) {}

		public List()
			: this(new List<Value>()) {}

		public List(List<Value> List)
		{
			Arr = List;
		}

		public List<Value> Arr { get; set; }

		public override Array ToArray()
		{
			return new Array(this);
		}
	}
}