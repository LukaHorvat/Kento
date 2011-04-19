using System;
using System.Collections.Generic;

namespace Kento
{
	internal class Function : CodeBlock, IFunction
	{
		private readonly List<String> args;
		private Scope scope;

		public Function ( Array Arguments, CodeBlock Code, Scope Scope )
			: this( Arguments.ToArray<String>(), Code.Value, Scope ) { }

		public Function ( List<String> Arguments, List<Token> Code, Scope Scope )
			: base( Code )
		{
			scope = Scope;
			args = Arguments;
			Type = CodeBlockType.Function;
		}

		public Scope Scope
		{
			get { return scope; }
			set { scope = value; }
		}

		#region IFunction Members

		public virtual Value Invoke ()
		{
			return Invoke( Array.Empty );
		}

		public virtual Value Invoke ( Array Args )
		{
			Compiler.SetAsCurrentScope( scope );
			Compiler.EnterScope();
			for ( int i = 0 ; i < Math.Min( Args.Arr.Count, args.Count ) ; ++i )
			{
				Compiler.SetAlias( args[ i ].Val, Args.Arr[ i ] );
			}
			Value result = Run();
			Compiler.ExitScope( true );
			return result;
		}

		#endregion

		public override Value Clone ()
		{
			return new Function( args, Value, scope );
		}
	}
}