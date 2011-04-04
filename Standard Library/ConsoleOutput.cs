using System;

namespace Kento
{
	class ConsoleOutput : ExternalFunction
	{
		public ConsoleOutput ()
			: base( "ConsoleOutput" ) { }
		public override Value Invoke ( Array Args )
		{
			foreach ( Value val in Args.Arr )
			{
				Console.Write( val.ToString() );
			}
			Console.WriteLine();
			return NoValue.Value;
		}
	}
}
