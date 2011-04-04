using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Reference : Value
	{
		Value referencingValue;
		public Value ReferencingValue
		{
			get { return referencingValue; }
		}

		public Reference ( Value ValueToReference )
		{
			referencingValue = ValueToReference;
		}
	}
}
