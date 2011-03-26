using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class CodeBlock : Value
	{
		List<Token> value;
		public List<Token> Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		Dictionary<string, Value> identifiers;
		public Dictionary<string, Value> Identifiers
		{
			get { return identifiers; }
			set { identifiers = value; }
		}

		public CodeBlock ( Expression Code )
		{
			value = Code.Tokenize();
			identifiers = new Dictionary<string, Value>();
		}
		public CodeBlock ( List<Token> Code )
		{
			value = Code;
			identifiers = new Dictionary<string, Value>();
		}

		public override Value Evaluate ()
		{
			return this;
		}

		public Value Run ()
		{
			if ( Compiler.ExecutingScope != null )
			{
				foreach ( var pair in Compiler.ExecutingScope.Identifiers )
				{
					if ( !identifiers.ContainsKey( pair.Key ) ) identifiers[ pair.Key ] = pair.Value;
				}
			}
			Compiler.ExecutingScope = this;

			Stack<Token> solvingStack = new Stack<Token>();
			for ( int i = 0 ; i < value.Count ; ++i )
			{
				if ( value[ i ] is Operator )
				{
					Operator op = (Operator)value[ i ];
					if ( op.Type == OperatorType.PrefixUnary || op.Type == OperatorType.SufixUnary )
					{
						op.Operate( ( solvingStack.Pop() as Value ), new NoValue() );
					} else
					{
						Value second = (Value)solvingStack.Pop();
						solvingStack.Push( op.Operate( ( solvingStack.Pop() as Value ), second ) );
					}
				} else
				{
					solvingStack.Push( value[ i ] );
				}
			}
			Compiler.ExitScope();
			if ( solvingStack.Count == 0 ) return new NoValue();
			else return (Value)solvingStack.Peek();
		}

		public override List<Token> Tokenize ()
		{
			return value;
		}
	}
}
