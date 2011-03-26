using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class ConsoleOutput : ExternalFunction
	{
		public override Value Invoke ( Array Args )
		{
			foreach ( Value val in Args.Arr )
			{
				Console.Write( val.ToString() );
			}
			Console.WriteLine();
			return new NoValue();
		}
	}
}
