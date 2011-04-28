using System;
using System.Collections.Generic;

namespace Kento
{
	internal class Function : CodeBlock, IInvokable
	{
		private readonly List<String> args;
		public Scope Scope { get; set; }
		public string Name { get; set; }

		public Function ( List Arguments, CodeBlock Code, Scope Scope )
			: this( Arguments.GetValues().ConvertAll( X => X as String ), Code.Value, Scope ) { }

		public Function ( List<String> Arguments, List<Token> Code, Scope Scope )
			: base( Code )
		{
			this.Scope = Scope;
			args = Arguments;
			Type = CodeBlockType.Function;
		}

		#region IInvokable Members

		public virtual Value Invoke ()
		{
			return Invoke( List.Empty );
		}

		public virtual Value Invoke ( List Args )
		{
			Compiler.SetAsCurrentScope( Scope );
			Compiler.EnterScope();
			for ( int i = 0 ; i < Math.Min( Args.Arr.Count, args.Count ) ; ++i )
			{
				if ( Args.Arr[ i ] is Reference )
				{
					( (Reference)new Identifier( args[ i ].Val ).Evaluate() ).ChangeReference( Args.Arr[ i ] as Reference );
				} else
				{
					( (Reference)new Identifier( args[ i ].Val ).Evaluate() ).ChangeReference( Args.Arr[ i ] );
				}
			}
			Value result = Run();
			Compiler.ExitScope( true );
			return result;
		}

		#endregion

		public override Value Clone ()
		{
			return new Function( args, Value, Scope );
		}

		public override string ToString ()
		{
			return "Function: " + Name;
		}
	}
}