using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Number : Literal
	{
		private double value;
		public override object Evaluate ()
		{
			return value;
		}
		public Number ( double Value )
		{
			value = Value;
		}
	}
}
