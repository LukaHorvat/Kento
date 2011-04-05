using System;

namespace Kento
{
	class ConsoleOutput : ExternalFunction
	{
		public ConsoleOutput ()
			: base( "ConsoleOutput" ) { }
		public override Value Invoke ( Array Args )
		{
			foreach ( Reference val in Args.Arr )
			{
				Console.Write( val.ReferencingValue.ToString() );
			}
			Console.WriteLine();
			return NoValue.Value;
		}
	}
}
