using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class String : Literal
	{
		private string value;
		public override object Evaluate ()
		{
			return value;
		}
		public String ( string Value )
		{
			value = Value;
		}
	}
}
