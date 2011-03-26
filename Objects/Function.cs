using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Function : CodeBlock
	{
		Array args;

		public Function ( Array Arguments, CodeBlock Code )
			: base( Code.Tokenize() )
		{
			args = Arguments;
		}
		public virtual Value Invoke ( Array Args )
		{
			for ( int i = 0 ; i < Math.Min(Args.Arr.Count, args.Arr.Count) ; ++i )
			{
				Identifiers[ ( args.Arr[ i ] as String ).Val ] = Args.Arr[ i ];
			}
			return Run();
		}
	}
}
