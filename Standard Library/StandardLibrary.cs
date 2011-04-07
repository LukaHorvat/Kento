using System;
using System.Collections.Generic;

namespace Kento
{
	class StandardLibrary
	{
		static readonly Dictionary<System.Type, Scope> defaultFunctions = new Dictionary<System.Type, Scope>();
		public Scope LoadFunctionsForType ( Value Value )
		{
			var type = Value.GetType();
			return defaultFunctions[ type ];
		}

	}
}
