using System.Collections.Generic;
using System.Text;

namespace Kento
{
	class Expression : Value
	{
		readonly Value first;
		readonly Value second;
		readonly Operator op;

		public static Value CreateValueFromExpression ( TokenList List )
		{
			if ( List.First == null ) return new Array();
			//Bracket pass
			var startNode = new TokenListNode();
			int parCount = 0;
			for ( var node = List.First ; node != null ; node = node.Next )
			{
				if ( node.Value is ParenthesisOpen )
				{
					if ( parCount == 0 )
					{
						startNode = node;
					}
					parCount++;
				} else if ( node.Value is ParenthesisClosed )
				{
					parCount--;
					if ( parCount == 0 )
					{
						var group = new TokenList();
						for ( var innerNode = startNode.Next ; innerNode != node ; innerNode = innerNode.Next )
						{
							group.Add( innerNode.Value );
						}
						var val = CreateValueFromExpression( group );
						List.Insert( new TokenListNode( val ), startNode.Previous, node.Next );
					}
				}
			}
			//Operator pass
			for ( int i = 1 ; i <= Operators.LowestPrecedance ; ++i )
			{
				for ( var node = List.First ; node != null ; node = node.Next )
				{
					if ( node.Value is Operator )
					{
						var oper = (Operator)node.Value;
						var type = oper.Type;
						if ( oper.Precedance == i )
						{
							var temp = new TokenList();
							TokenListNode left = new TokenListNode(), right = new TokenListNode();
							if ( oper is DotOperator ) type = OperatorType.InfixBinary;
							switch ( type )//Using the operators type, determines which tokens to make a value from
							{
								case OperatorType.InfixBinary: //A + B
									temp.Add( node.Previous.Value );
									temp.Add( node.Value );
									temp.Add( node.Next.Value );
									left = node.Previous.Previous;
									right = node.Next.Next;
									break;
								case OperatorType.PrefixBinary: //+ A B
									temp.Add( node.Next.Value );
									temp.Add( node.Value );
									temp.Add( node.Next.Next.Value );
									left = node.Previous;
									right = node.Next.Next.Next;
									break;
								case OperatorType.PrefixUnary: //+ A
									temp.Add( node.Value );
									temp.Add( node.Next.Value );
									left = node.Previous;
									right = node.Next.Next;
									break;
								case OperatorType.SufixBinary://A B +
									temp.Add( node.Previous.Previous.Value );
									temp.Add( node.Value );
									temp.Add( node.Previous.Value );
									left = node.Previous.Previous.Previous;
									right = node.Next;
									break;
								case OperatorType.SufixUnary: //A +
									temp.Add( node.Value );
									temp.Add( node.Previous.Value );
									left = node.Previous.Previous;
									right = node.Next;
									break;
							}
							var val = MakeValue( temp, ( type == OperatorType.PrefixUnary || type == OperatorType.SufixUnary ) );
							List.Insert( new TokenListNode( val ), left, right );
						}
					}
				}
			}
			if ( List.First.Next != null )
			{
				return new ExpressionSequence( List );
			}
			return (Value)List.First.Value;
		}
		static Value MakeValue ( TokenList List, bool Unary )
		{
			if ( Unary )
			{
				Value result = NoValue.Value;
				if ( List.First.Value is ICanRunAtCompile )
				{
					result = ( (ICanRunAtCompile)List.First.Value ).CompileTimeOperate( (Value)List.First.Next.Value, NoValue.Value );
				}
				if ( result is NoValue )
				{
					return new Expression( (Value)List.First.Next.Value, NoValue.Value, (Operator)List.First.Value );
				}
				return result;
			} else
			{
				Value result = NoValue.Value;
				if ( List.First.Next.Value is ICanRunAtCompile )
				{
					result = ( (ICanRunAtCompile)List.First.Next.Value ).CompileTimeOperate( (Value)List.First.Value, (Value)List.First.Next.Next.Value );
				}
				if ( result is NoValue )
				{
					return new Expression( (Value)List.First.Value, (Value)List.First.Next.Next.Value, (Operator)List.First.Next.Value );
				}
				return result;
			}
		}
		public Expression ( Value First, Value Second, Operator Operator )
		{
			first = First;
			second = Second;
			op = Operator;
		}
		public override string ToString ()
		{
			var builder = new StringBuilder();

			builder.Append( first.ToString() );
			builder.Append( " " );
			if ( !( second is NoValue ) )
			{
				builder.Append( second.ToString() );
				builder.Append( " " );
			}
			builder.Append( op.ToString() );
			builder.Append( " " );
			return builder.ToString();
		}
		public override Value Evaluate ()
		{
			return this;
		}
		public override List<Token> Tokenize ()
		{
			var rpn = new List<Token>();
			if ( op is DotOperator ) //Special case for the dot operator which needs to be arranged like a sufix unary
			{
				rpn.Add( first );
				rpn.Add( op );
				rpn.Add( second );
			} else
			{
				if ( first is Expression || first is ExpressionSequence ) rpn.AddRange( first.Tokenize() );
				else rpn.Add( first );
				if ( second is Expression || second is ExpressionSequence ) rpn.AddRange( second.Tokenize() );
				else if ( !( second is NoValue ) ) rpn.Add( second );
				rpn.Add( op );
			}

			return rpn;
		}
	}
}
