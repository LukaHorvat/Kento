using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Array : Value
	{
		List<Value> arr;
		public List<Value> Arr
		{
			get { return arr; }
			set { arr = value; }
		}
		public Array ( params Value[] Values )
			: base()
		{
			arr = new List<Value>( Values );
		}
		public Array ()
		{
			arr = new List<Value>();
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { this } );
		}
		public override Array ToArray ()
		{
			return this;
		}
	}
}
