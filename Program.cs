using System;

namespace Kento
{
	internal class Program
	{
		private static void Main ( string[] args )
		{
			if ( args.Length == 0 )
			{
				Console.WriteLine( "Please provide the path to the source code as a command-line argument" );
				Console.ReadKey();
				return;
			}
			Compiler.RunFromFile( args[ 0 ], new DebugOptions
			{
				OutputMemoryAllocation = false,
				OutputProfilerInfo = true,
				OutputDestruction = false
			} );
			Console.ReadKey();
		}
	}
}