﻿using System;

namespace Kento
{
	class ConsoleInput : ExternalFunction
	{
		public ConsoleInput ()
			: base( "ConsoleInput" ) { }
		public override Value Invoke ( Array Args )
		{
			return new String( Console.ReadLine() );
		}
	}
}
