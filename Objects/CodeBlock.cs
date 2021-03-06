﻿using System.Collections.Generic;

namespace Kento
{
	internal enum CodeBlockType
	{
		Function = 0,
		Loop,
		Type,
		Other
	}

	internal class CodeBlock : Value
	{
		private CodeBlockType type = CodeBlockType.Other;
		private List<Token> value;

		public CodeBlock ( Expression Code )
		{
			value = Code.Tokenize();
		}

		public CodeBlock ( List<Token> Code )
		{
			value = Code;
		}

		public List<Token> Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public CodeBlockType Type
		{
			get { return type; }
			set { type = value; }
		}

		public override Value Evaluate ()
		{
			return this;
		}

		public virtual Value Run ()
		{
			if ( type == CodeBlockType.Other )
			{
				Compiler.EnterScope();
			}
			var solvingStack = new Stack<Value>();
			for ( int i = 0 ; i < value.Count ; ++i )
			{
				if ( value[ i ] is Operator )
				{
					var op = (Operator)value[ i ];
					if ( op is ContinueOperator )
					{
						Compiler.Fallthrough = FallThroughType.Continue;
					} else if ( op is BreakOperator )
					{
						Compiler.Fallthrough = FallThroughType.Break;
					} else
					{
						if ( op is ReturnOperator )
						{
							Compiler.Fallthrough = FallThroughType.Return;
						}
						Value second = NoValue.Value;
						if ( op.Type != OperatorType.PrefixUnary && op.Type != OperatorType.SufixUnary )
						{
							second = solvingStack.Pop();
						}
						Value result = op.Operate( solvingStack.Pop(), second );
						if ( result != Nothing.Value )
							solvingStack.Push( result );
					}
				} else
				{
					solvingStack.Push( ( (Value)value[ i ] ).Evaluate() );
				}
				if ( Compiler.Fallthrough != FallThroughType.NoFallthrough )
				{
					if ( type == CodeBlockType.Function && Compiler.Fallthrough == FallThroughType.Return )
					{
						Compiler.Fallthrough = FallThroughType.NoFallthrough;
						break;
					}
					if ( type == CodeBlockType.Loop )
					{
						if ( Compiler.Fallthrough == FallThroughType.Continue )
						{
							Compiler.Fallthrough = FallThroughType.NoFallthrough;
							i = -1;
							solvingStack.Clear();
							continue;
						}
						if ( Compiler.Fallthrough == FallThroughType.Break )
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
			if ( solvingStack.Count > 0 ) toReturn = solvingStack.Peek().Evaluate();
			if ( toReturn is Reference ) toReturn = ( toReturn as Reference ).ReferencingValue;

			Compiler.StoreValuesOutOfScope = true; //Make sure the returning value is not destroyed when exiting the function's scope
			if ( toReturn is Literal ) toReturn = toReturn.Clone();
			Compiler.StoreValuesOutOfScope = false;

			if ( type != CodeBlockType.Loop && type != CodeBlockType.Type ) Compiler.ExitScope();
			return toReturn;
		}
	}
}