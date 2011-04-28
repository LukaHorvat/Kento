using System;

namespace Kento
{
	internal class SLConsole : ILibrarySegment
	{
		#region ILibrarySegment Members

		public ExternalClass Load ()
		{
			var output = new ExternalFunction( "Output", true, Output );
			var input = new ExternalFunction( "Input", true, Input );

			return new ExternalClass( "Console", InstanceFlags.NoFlags, output, input,
				new ExternalFunction( "Clear", true, Clear ) );
		}

		#endregion

		public Value Output ( List Arguments )
		{
			foreach ( var val in Arguments.GetValues() )
			{
				Console.Write( val );
			}
			Console.WriteLine();
			return NoValue.Value;
		}

		public Value Input ( List Arguments )
		{
			return new String( Console.ReadLine() );
		}

		public Value Clear ( List Arguments )
		{
			Console.Clear();
			return NoValue.Value;
		}
	}
}