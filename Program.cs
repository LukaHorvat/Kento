﻿using System;
using Kento.Utility;

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
			Compiler.RunFromFile( args[ 0 ], RuntimeFlags.Debug );
			Console.ReadKey();
		}
	}
}
