﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class ConsoleInput:ExternalFunction
	{
		public override Value Invoke ( Array Args )
		{
			return new String( Console.ReadLine() );
		}
	}
}