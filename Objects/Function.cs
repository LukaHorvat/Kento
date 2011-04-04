using System;

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
				Compiler.SetAliasInCurrentScope( ( args.Arr[ i ] as String ).Val, new Reference( Args.Arr[ i ].Evaluate().Clone() ) );
			}
			var result = Run();
			return result;
		}
	}
}
