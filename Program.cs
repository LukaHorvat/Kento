﻿using System;
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
			System.Threading.Thread.Sleep( 1000 );
			using ( StreamReader reader = new StreamReader( @"scripts\FirstTestScript.kt" ) )
			{
				Compiler.Run( reader.ReadToEnd() );
			}
			Console.WriteLine( Compiler.LastRunningTime );
			Console.ReadKey();
		}
	}
}
