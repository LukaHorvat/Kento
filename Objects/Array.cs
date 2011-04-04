using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public class Array : Value
	{
		List<Value> arr;
		public List<Value> Arr
		{
			get { return arr; }
			set { arr = value; }
		}
		public Array ( params Value[] Values )
		{
			arr = new List<Value>( Values );
		}
		public Array ( List<Value> List )
		{
			arr = List;
		}
		public Array ()
		{
			arr = new List<Value>();
		}
		public override Array ToArray ()
		{
			return this;
		}
		public override Value Clone ()
		{
			var list = new List<Value>();
			foreach ( var val in arr )
			{
				list.Add( val.Clone() );
			}
			return new Array( list );
		}
	}
}
