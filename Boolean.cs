using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Boolean : Literal
	{
		private bool value;
		public override object Evaluate ()
		{
			return value;
		}
		public Boolean ( bool Value )
		{
			value = Value;
		}
	}
}
