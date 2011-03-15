using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kento
{
	class Program
	{
		static void Main ( string[] args )
		{
			Tokenizer.GetRPN( "23 + 45 * (27 / (\"apple pie\" + (12 * 2) ) ) * a <= abrakadabra( 12 == 8 )" );
			Console.ReadKey();
		}
	}
}
