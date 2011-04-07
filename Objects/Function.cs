using System;
using System.Collections.Generic;

namespace Kento
{
	class Function : CodeBlock
	{
		List<String> args;
		private Scope scope;
		public Scope Scope
		{
			get { return scope; }
			set { scope = value; }
		}

		public Function ( Array Arguments, CodeBlock Code, Scope Scope )
			: base( Code.Value )
		{
			scope = Scope;
			args = new List<String>();
			foreach ( var reference in Arguments.Arr )
			{
				if ( reference.ReferencingValue is String )
				{
					args.Add( (String)reference.ReferencingValue );
				} else throw new Exception( "Function requires a list of strings" );
			}
			Type = CodeBlockType.Function;
		}
		public Function ( List<String> Arguments, List<Token> Code, Scope Scope )
			: base( Code )
		{
			scope = Scope;
			args = Arguments;
			Type = CodeBlockType.Function;
		}
		public virtual Value Invoke ( Array Args )
		{
			Compiler.SetAsCurrentScope( scope );
			Compiler.EnterScope();
			for ( int i = 0 ; i < Math.Min( Args.Arr.Count, args.Count ) ; ++i )
			{
				Compiler.SetAlias( args[ i ].Val, Args.Arr[ i ] );
			}
			var result = Run();
			Compiler.ExitScope( true );
			return result;
		}
		public override Value Clone ()
		{
			return new Function( args, Value, scope );
		}
	}
}
