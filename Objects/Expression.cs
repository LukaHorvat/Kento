using System.Collections.Generic;
using System.Text;

namespace Kento
{
	class Expression : Value
	{
		Value first, second;
		Operator op;
		bool unary;

		public static Value CreateValueFromExpression ( TokenList List )
		{
			if ( List.First == null ) return new Array();
			//Bracket pass
			TokenListNode startNode = new TokenListNode();
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
						TokenList group = new TokenList();
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
						if ( oper.Precedance == i )
						{
							TokenList temp = new TokenList();
							TokenListNode left = new TokenListNode(), right = new TokenListNode();
							switch ( oper.Type )//Using the operators type, determines which tokens to make a value from
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
							var val = makeValue( temp, ( oper.Type == OperatorType.PrefixUnary || oper.Type == OperatorType.SufixUnary ) );
							List.Insert( new TokenListNode( val ), left, right );
						}
					}
				}
			}
			if ( List.First.Next != null )
			{
				return new ExpressionSequence( List );
			} else
				return (Value)List.First.Value;
		}
		static Value makeValue ( TokenList List, bool Unary )
		{
			if ( Unary )
			{
				Value result = NoValue.Value;
				if ( !( ( List.First.Value as Operator ) is IRequiresRuntime ) )
				{
					result = ( (Operator)List.First.Value ).Operate( (Value)List.First.Next.Value, NoValue.Value );
				}
				if ( result is NoValue )
				{
					return new Expression( (Value)List.First.Next.Value, NoValue.Value, (Operator)List.First.Value, true );
				} else return result;
			} else
			{
				Value result = NoValue.Value;
				if ( !( ( List.First.Next.Value as Operator ) is IRequiresRuntime ) )
				{
					result = ( (Operator)List.First.Next.Value ).Operate( (Value)List.First.Value, (Value)List.First.Next.Next.Value );
				}
				if ( result is NoValue )
				{
					return new Expression( (Value)List.First.Value, (Value)List.First.Next.Next.Value, (Operator)List.First.Next.Value, false );
				} else return result;
			}
		}
		public Expression ( Value First, Value Second, Operator Operator, bool Unary )
		{
			first = First;
			second = Second;
			op = Operator;
			unary = Unary;
		}
		public override string ToString ()
		{
			StringBuilder builder = new StringBuilder();

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
			if ( first is Expression ) rpn.AddRange( ( first as Expression ).Tokenize() );
			else rpn.Add( first );
			if ( second is Expression ) rpn.AddRange( ( second as Expression ).Tokenize() );
			else if ( !( second is NoValue ) ) rpn.Add( second );
			rpn.Add( op );

			return rpn;
		}
	}
}
