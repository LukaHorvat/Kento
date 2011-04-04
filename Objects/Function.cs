using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kento
{
	class Function : CodeBlock
	{
		Array args;

		public Function ( Array Arguments, CodeBlock Code )
			: base( Code.Value )
		{
			args = Arguments;
			Type = CodeBlockType.Function;
		}
		public virtual Value Invoke ( Array Args )
		{
			Compiler.EnterScope();
			for ( int i = 0 ; i < Math.Min( Args.Arr.Count, args.Arr.Count ) ; ++i )
			{
				Compiler.MakeValueInCurrentScope( ( args.Arr[ i ] as String ).Val, Args.Arr[ i ].Evaluate().Clone() );
			}
			var result = Run();
			return result;
		}
	}
}
