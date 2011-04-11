using System;

namespace Kento
{
	class SLConsole : ILibrarySegment
	{
		public ExternalClass Load ()
		{
			var output = new ExternalFunction( "Output", true, Output );
			var input = new ExternalFunction( "Input", true, Input );

			return new ExternalClass( "Console", InstanceFlags.NoFlags, output, input );
		}
		public Value Output ( Array Arguments )
		{
			foreach ( var val in Arguments.Arr )
			{
				Console.Write( val.ReferencingValue.ToString() );
			}
			Console.WriteLine();
			return NoValue.Value;
		}
		public Value Input ( Array Arguments )
		{
			return new String( Console.ReadLine() );
		}
	}
}
