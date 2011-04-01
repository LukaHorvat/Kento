using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Instance : Value
	{
		Dictionary<string, Value> identifiers;
		public Dictionary<string, Value> Identifiers
		{
			get { return identifiers; }
			set { identifiers = value; }
		}

		public Instance ( Type Type )
		{
			identifiers = Type.Identifiers.Clone();
		}
	}
}
