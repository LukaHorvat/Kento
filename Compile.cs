using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Compile
	{
		Dictionary<string, object> variables;
		public Dictionary<string, object> Variables
		{
			get { return variables; }
			set { variables = value; }
		}
	}
}
