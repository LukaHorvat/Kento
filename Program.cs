using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kento
{
	class Program
	{
		static void Main ( string[] args )
		{
			if ( args.Length == 0 )
			{
				Console.WriteLine( "Please provide the path to the source code as a command-line argument" );
				Console.ReadKey();
				return;
			}
			using ( StreamReader reader = new StreamReader( args[ 0 ] ) )
			{
				Compiler.Run( reader.ReadToEnd() );
			}
			Console.ReadKey();
		}
	}
}
