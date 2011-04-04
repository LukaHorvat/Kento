using System.Collections.Generic;

namespace Kento
{
	enum CodeBlockType
	{
		Function = 0,
		Loop,
		Other
	}
	class CodeBlock : Value
	{
		List<Token> value;
		public List<Token> Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		CodeBlockType type = CodeBlockType.Other;
		public CodeBlockType Type
		{
			get { return type; }
			set { type = value; }
		}

		public CodeBlock ( Expression Code )
		{
			value = Code.Tokenize();
		}
		public CodeBlock ( List<Token> Code )
		{
			value = Code;
		}

		public override Value Evaluate ()
		{
			return this;
		}

		public virtual Value Run ()
		{
			if ( this is Function == false && this is Type == false )
			{
				Compiler.EnterScope();
			}
			Stack<Token> solvingStack = new Stack<Token>();
			for ( int i = 0 ; i < value.Count ; ++i )
			{
				if ( value[ i ] is Operator )
				{
					Operator op = (Operator)value[ i ];
					if ( op is ReturnOperator )
					{
						Compiler.Fallthrough = FallThroughType.Return;
					} else if ( op is ContinueOperator )
					{
						Compiler.Fallthrough = FallThroughType.Continue;
					} else if ( op is BreakOperator )
					{
						Compiler.Fallthrough = FallThroughType.Break;
					} else
					{
						Value second = NoValue.Value;
						if ( op.Type != OperatorType.PrefixUnary && op.Type != OperatorType.SufixUnary )
						{
							second = (Value)solvingStack.Pop();
						}
						Value result = op.Operate( ( solvingStack.Pop() as Value ), second );
						solvingStack.Push( result );
					}
				} else
				{
					solvingStack.Push( value[ i ] );
				}
				if ( Compiler.Fallthrough != FallThroughType.NoFallthrough )
				{
					if ( type == CodeBlockType.Function && Compiler.Fallthrough == FallThroughType.Return )
					{
						Compiler.Fallthrough = FallThroughType.NoFallthrough;
						break;
					} else if ( type == CodeBlockType.Loop )
					{
						if ( Compiler.Fallthrough == FallThroughType.Continue )
						{
							Compiler.Fallthrough = FallThroughType.NoFallthrough;
							i = -1;
							solvingStack.Clear();
							continue;
						} else if ( Compiler.Fallthrough == FallThroughType.Break )
						{
							Compiler.Fallthrough = FallThroughType.NoFallthrough;
							break;
						}
					} else
					{
						break;
					}
				}
			}
			Value toReturn = NoValue.Value;
			if ( solvingStack.Count > 0 ) toReturn = ( solvingStack.Peek() as Value ).Evaluate();
			Compiler.ExitScope( this is Type );
			return toReturn;
		}
	}
}
