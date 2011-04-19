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

		public Value Output ( Array Arguments )
		{
			foreach ( Reference val in Arguments.Arr )
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

		public Value Clear ( Array Arguments )
		{
			Console.Clear();
			return NoValue.Value;
		}
	}
}