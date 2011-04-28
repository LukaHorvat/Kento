﻿using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable RedundantUsingDirective

// ReSharper restore RedundantUsingDirective

namespace Kento
{
	internal abstract class Operator : Token
	{
		protected Operator ( int Precedance, OperatorType Type )
		{
			this.Precedance = Precedance;
			this.Type = Type;
		}

		public int Precedance { get; set; }

		public OperatorType Type { get; set; }

		public override string ToString ()
		{
			return Operators.RepresentationDictionary[ GetType() ];
		}

		public abstract Value Operate ( Value First, Value Second );
	}

	public struct BracketType
	{
		private System.Type closingBracket;
		private System.Type openingBracket;
		private System.Type specialOperator;

		public BracketType ( System.Type Open, System.Type Closed, System.Type Special )
		{
			openingBracket = Open;
			closingBracket = Closed;
			specialOperator = Special;
		}

		public System.Type SpecialOperator
		{
			get { return specialOperator; }
			set { specialOperator = value; }
		}

		public System.Type ClosingBracket
		{
			get { return closingBracket; }
			set { closingBracket = value; }
		}

		public System.Type OpeningBracket
		{
			get { return openingBracket; }
			set { openingBracket = value; }
		}
	}

	public class Operators
	{
		static Operators ()
		{
			OperatorDictionary = new Dictionary<string, System.Type>();
			RepresentationDictionary = new Dictionary<System.Type, string>();
			Brackets = new LinkedList<BracketType>();

			Add( "++", typeof( Increment ) );
			Add( "--", typeof( Decrement ) );
			Add( "-=", typeof( SubtractiveAssignment ) );
			Add( "-", typeof( Subtraction ) );
			Add( "(", typeof( ParenthesisOpen ) );
			Add( ")", typeof( ParenthesisClosed ) );
			Add( "!", typeof( NotOperator ) );
			Add( "!=", typeof( NotEqualTo ) );
			Add( "*=", typeof( MultiplicativeAssignment ) );
			Add( "*", typeof( Multiplication ) );
			Add( "%", typeof( ModOperator ) );
			Add( "%=", typeof( ModAssignment ) );
			Add( "||", typeof( LogicalOr ) );
			Add( "&&", typeof( LogicalAnd ) );
			Add( "<", typeof( LessThan ) );
			Add( "<=", typeof( LessOrEqual ) );
			Add( ">", typeof( GreaterThan ) );
			Add( ">=", typeof( GreaterOrEqual ) );
			Add( "==", typeof( EqualTo ) );
			Add( "+=", typeof( AdditiveAssignment ) );
			Add( "=", typeof( Assignment ) );
			Add( "/", typeof( Division ) );
			Add( "/=", typeof( DivisiveAssignment ) );
			Add( "+", typeof( Addition ) );
			Add( ".", typeof( DotOperator ) );
			Add( "?", typeof( Unidentified ) );
			Add( "if", typeof( IfOperator ) );
			Add( "else", typeof( ElseOperator ) );
			Add( "{", typeof( CurlyBracesOpen ) );
			Add( "}", typeof( CurlyBracesClosed ) );
			Add( ",", typeof( CommaOperator ) );
			Add( "function", typeof( FunctionOperator ) );
			Add( "return", typeof( ReturnOperator ) );
			Add( "continue", typeof( ContinueOperator ) );
			Add( "break", typeof( BreakOperator ) );
			Add( "while", typeof( WhileOperator ) );
			Add( "[", typeof( SquareBracketsOpen ) );
			Add( "]", typeof( SquareBracketsClosed ) );
			Add( "class", typeof( ClassOperator ) );
			Add( "new", typeof( NewOperator ) );
			Add( "&", typeof( ReferenceOperator ) );
			Add( "&=", typeof( ReferenceAssignment ) );
			Add( "typeof", typeof( TypeofOperator ) );
			Add( "declare", typeof( DeclareOperator ) );
			Add( "static", typeof( StaticOperator ) );

			RepresentationDictionary.Add( typeof( InvokeOperator ), "InvokeOperator" );
			RepresentationDictionary.Add( typeof( MakeCodeBlock ), "MakeCodeBlock" );
			RepresentationDictionary.Add( typeof( AccessValueAtIndex ), "SquareBracketsOperator" );

			LowestPrecedance = RepresentationDictionary.Keys.Max( X => ( (Operator)Activator.CreateInstance( X ) ).Precedance );

			Brackets.AddLast( new BracketType( typeof( CurlyBracesOpen ), typeof( CurlyBracesClosed ), typeof( MakeCodeBlock ) ) );
			Brackets.AddLast( new BracketType( typeof( SquareBracketsOpen ), typeof( SquareBracketsClosed ),
											 typeof( AccessValueAtIndex ) ) );
		}

		public static int LowestPrecedance { get; set; }

		public static Dictionary<string, System.Type> OperatorDictionary { get; set; }

		public static Dictionary<System.Type, string> RepresentationDictionary { get; set; }

		public static LinkedList<BracketType> Brackets { get; set; }

		private static void Add ( string Rep, System.Type Type )
		{
			OperatorDictionary.Add( Rep, Type );
			RepresentationDictionary.Add( Type, Rep );
		}
	}

	internal class SubtractiveAssignment : Operator
	{
		public SubtractiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				Value value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val - ( value2 as Number ).Val );
					Value toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}

	internal class DivisiveAssignment : Operator
	{
		public DivisiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				Value value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val / ( value2 as Number ).Val );
					Value toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}

	internal class MultiplicativeAssignment : Operator
	{
		public MultiplicativeAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				Value value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val * ( value2 as Number ).Val );
					Value toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}

	internal class AdditiveAssignment : Operator
	{
		public AdditiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				Value value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val + ( value2 as Number ).Val );
					return First;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}

	internal class ModAssignment : Operator
	{
		public ModAssignment ()
			: base( 14, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				Value value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val % ( value2 as Number ).Val );
					Value toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}

	internal class ReferenceAssignment : Operator
	{
		public ReferenceAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference && Second is Reference )
			{
				( First as Reference ).ChangeReference( Second as Reference );
				return Second;
			}
			throw new Exception( "Both operands must be identifiers" );
		}
	}

	internal class Assignment : Operator
	{
		public Assignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Reference )
			{
				if ( Second is INamable ) ( Second as INamable ).Name = ( First as Reference ).Identifier; //Give the object a name for easier debugging
				Value toSet;
				if ( Second is List ) toSet = Second.ToList();
				else toSet = Second is Literal ? Second.Clone() : Second;

				( First as Reference ).ReferencingValue = toSet;
				return toSet;
			}
			throw new Exception( "Operands must be an identifier and a value" );
		}
	}

	internal class Subtraction : Operator, ICanRunAtCompile
	{
		public Subtraction ()
			: base( 4, OperatorType.InfixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}

	internal class Division : Operator, ICanRunAtCompile
	{
		public Division ()
			: base( 3, OperatorType.InfixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}

	internal class Addition : Operator, ICanRunAtCompile
	{
		public Addition ()
			: base( 4, OperatorType.InfixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First == NoValue.Value || Second == NoValue.Value
				|| First is Expression || Second is Expression
				|| First is ExpressionSequence || Second is ExpressionSequence
				|| First is CodeBlock || Second is CodeBlock )
			{
				return NoValue.Value;
			}
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val + ( Second as Number ).Val );
			}
			if ( First is String || Second is String )
			{
				return new String( First.ToString() + Second );
			}
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val + ( Second as Number ).Val );
			}
			if ( First is String || Second is String )
			{
				return new String( First.ToString() + Second );
			}
			throw new Exception( "Both operands must be numbers or one of the operands must be a string" );
		}
	}

	internal class Multiplication : Operator, ICanRunAtCompile
	{
		public Multiplication ()
			: base( 3, OperatorType.InfixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}

	internal class ModOperator : Operator, ICanRunAtCompile
	{
		public ModOperator ()
			: base( 3, OperatorType.InfixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( (int)( First as Number ).Val % (int)( Second as Number ).Val );
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val % ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}

	internal class ParenthesisOpen : Operator
	{
		public ParenthesisOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class ParenthesisClosed : Operator
	{
		public ParenthesisClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class CurlyBracesOpen : Operator
	{
		public CurlyBracesOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class CurlyBracesClosed : Operator
	{
		public CurlyBracesClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class SquareBracketsOpen : Operator
	{
		public SquareBracketsOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class SquareBracketsClosed : Operator
	{
		public SquareBracketsClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class InvokeOperator : Operator
	{
		public InvokeOperator ()
			: base( 2, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference && !( Second is HardReference ) ) Second = ( Second as Reference ).ReferencingValue;
			var list = Second.ToList();

			if ( First is IInvokable )
			{
				Value toReturn = ( First as IInvokable ).Invoke( list );
				list.Destroy();
				if ( toReturn is HardReference ) toReturn = ( toReturn as HardReference ).ToNormalReference();
				return toReturn;
			}
			throw new Exception( "First operand must be a function" );
		}
	}

	internal class MakeCodeBlock : Operator, ICanRunAtCompile
	{
		public MakeCodeBlock ()
			: base( 13, OperatorType.PrefixUnary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			return Operate( First, Second );
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is Expression )
			{
				return new CodeBlock( ( First as Expression ) );
			}
			if ( First is ExpressionSequence )
			{
				return new CodeBlock( ( First as ExpressionSequence ).Tokenize() );
			}
			throw new Exception( "Operand must be an expression or an expression sequence" );
		}
	}

	internal class MakeArray : Operator
	{
		public MakeArray ()
			: base( 13, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return First.ToList();
		}
	}

	internal class AccessValueAtIndex : Operator
	{
		public AccessValueAtIndex ()
			: base( 2, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is IIndexable && Second is Number )
			{
				var array = First as IIndexable;
				var index = (int)( Second as Number ).Val;
				Reference toReturn = array.GetReferenceAtIndex( index );
				return toReturn;
			}
			throw new Exception( "First operand must be indexable and the second one must be a number" );
		}
	}

	internal class NotOperator : Operator, ICanRunAtCompile
	{
		public NotOperator ()
			: base( 1, OperatorType.PrefixUnary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			throw new Exception( "Operand must be a boolean" );
		}
	}

	internal class ReferenceOperator : Operator
	{
		public ReferenceOperator ()
			: base( 1, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				return ( First as Reference ).GetHardReference();
			}
			throw new Exception( "Reference operator expects an identifier" );
		}
	}

	internal class TypeofOperator : Operator
	{
		public TypeofOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			return new String( First.GetType().ToString() );
		}
	}

	internal class StaticOperator : Operator
	{
		public StaticOperator ()
			: base( 16, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			First.Static = true;
			return First;
		}
	}

	internal class NotEqualTo : Operator
	{
		public NotEqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val != ( (Number)Second ).Val );
				if ( First is String ) return new Boolean( ( First as String ).Val != ( (String)Second ).Val );
				if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val != ( (Boolean)Second ).Val );
				throw new Exception( "Operands are not comparable" );
			}
			return new Boolean( true );
		}
	}

	internal class EqualTo : Operator
	{
		public EqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( !( First is NoValue || Second is NoValue ) && First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val == ( (Number)Second ).Val );
				if ( First is String ) return new Boolean( ( First as String ).Val == ( (String)Second ).Val );
				if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val == ( (Boolean)Second ).Val );
				throw new Exception( "Operands are not comparable" );
			}
			throw new Exception( "Operands are not comparable" );
		}
	}

	internal class LessThan : Operator
	{
		public LessThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				var toReturn = new Boolean( ( First as Number ).Val < ( Second as Number ).Val );
				return toReturn;
			}
			if ( Compiler.Runtime ) throw new Exception( "Operands must be numbers" );
			return NoValue.Value;
		}
	}

	internal class LessOrEqual : Operator
	{
		public LessOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val <= ( Second as Number ).Val );
			throw new Exception( "Operands must be numbers" );
		}
	}

	internal class GreaterThan : Operator
	{
		public GreaterThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val > ( Second as Number ).Val );
			throw new Exception( "Operands must be numbers" );
		}
	}

	internal class GreaterOrEqual : Operator
	{
		public GreaterOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val >= ( Second as Number ).Val );
			throw new Exception( "Operands must be numbers" );
		}
	}

	internal class LogicalOr : Operator
	{
		public LogicalOr ()
			: base( 12, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val || ( Second as Boolean ).Val );
			throw new Exception( "Operands must be boolean" );
		}
	}

	internal class LogicalAnd : Operator
	{
		public LogicalAnd ()
			: base( 11, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val && ( Second as Boolean ).Val );
			throw new Exception( "Operands must be boolean" );
		}
	}

	internal class Increment : Operator
	{
		public Increment ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				if ( ( First as Reference ).ReferencingValue is Number )
				{
					var oldNumber = ( ( First as Reference ).ReferencingValue as Number );
					var toReturn = new Number( oldNumber.Val + 1 );
					( First as Reference ).ReferencingValue = toReturn;
					return oldNumber;
				}
				throw new Exception( "Operand must be a number" );
			}
			throw new Exception( "Operand must be an identifier" );
		}
	}

	internal class Decrement : Operator
	{
		public Decrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				if ( ( First as Reference ).ReferencingValue is Number )
				{
					var oldNumber = ( ( First as Reference ).ReferencingValue as Number );
					var toReturn = new Number( oldNumber.Val - 1 );
					( First as Reference ).ReferencingValue = toReturn;
					return oldNumber;
				}
				throw new Exception( "Operand must be a number" );
			}
			throw new Exception( "Operand must be an identifier" );
		}
	}

	internal class Unidentified : Operator
	{
		public Unidentified ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class IfOperator : Operator
	{
		public IfOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean && Second is CodeBlock )
			{
				if ( ( First as Boolean ).Val )
				{
					Value toReturn = ( Second as CodeBlock ).Run();
					return toReturn;
				}
				return new ConditionNotMet();
			}
			throw new Exception( "Operands must be a boolean and a code block" );
		}
	}

	internal class ElseOperator : Operator
	{
		public ElseOperator ()
			: base( 16, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( Second is CodeBlock )
			{
				if ( First is ConditionNotMet ) return ( Second as CodeBlock ).Run();
				return NoValue.Value;
			}
			throw new Exception( "Else must be followed by a code block" );
		}
	}

	internal class WhileOperator : Operator, ICanRunAtCompile
	{
		public WhileOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		#region ICanRunAtCompile Members

		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Expression ) First = new CodeBlock( First as Expression );
			if ( First is CodeBlock && Second is CodeBlock )
			{
				return new Loop( First as CodeBlock, Second as CodeBlock );
			}
			return NoValue.Value;
		}

		#endregion

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean ) First = new CodeBlock( new[] { First as Token }.ToList() );
			if ( First is Expression ) First = new CodeBlock( First as Expression );
			if ( First is CodeBlock && Second is CodeBlock )
			{
				return new Loop( First as CodeBlock, Second as CodeBlock );
			}
			throw new Exception( "Operands must be code blocks" );
		}
	}

	internal class FunctionOperator : Operator
	{
		public FunctionOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			var arr = First.ToList();

			if ( Second is CodeBlock )
			{
				var code = ( Second as CodeBlock );
				return new Function( arr, code, Compiler.GetCurrentScope() );
			}
			throw new Exception( "Function must be followed by a list and a code block" );
		}
	}

	internal class ClassOperator : Operator
	{
		public ClassOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Array && ( First as Array ).Arr.Count == 0 && Second is CodeBlock )
			{
				var newType = new Type( ( Second as CodeBlock ) );
				newType.Run();
				return newType;
			}
			if ( First is Type && Second is CodeBlock )
			{
				var newType = new Type( ( First as Type ), ( Second as CodeBlock ) );
				return newType.Run();
			}
			throw new Exception( "First operand must be a type or an empty list and the second operand must bea code block" );
		}
	}

	internal class DeclareOperator : Operator
	{
		public DeclareOperator ()
			: base( 14, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return First;
		}
	}

	internal class NewOperator : Operator
	{
		public NewOperator ()
			: base( 1, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is IClass )
			{
				return ( First as IClass ).MakeInstance();
			}
			throw new Exception( "Operand must be a type" );
		}
	}

	internal class CommaOperator : Operator
	{
		public CommaOperator ()
			: base( 13, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is List )
			{
				( First as List ).Arr.Add( Second is Literal ? Second.Clone() : Second );
				return First;
			}
			return new List( First is Literal ? First.Clone() : First,
				Second is Literal ? Second.Clone() : Second );
		}
	}

	internal class DotOperator : Operator
	{
		public DotOperator ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is IHasMembers )
			{
				var inst = First as IHasMembers;
				Compiler.SetAsCurrentScope( inst.Identifiers );
				Compiler.PendingDot = true;
				Compiler.PendingDotIsStatic = First is IClass;
				return Nothing.Value;
			}
			throw new Exception( "Operand must have members" );
		}
	}

	internal class ReturnOperator : Operator
	{
		public ReturnOperator ()
			: base( 15, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new Reference( First ).GetHardReference();
		}
	}

	internal class ContinueOperator : Operator
	{
		public ContinueOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class BreakOperator : Operator
	{
		public BreakOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	internal class RunCodeBlock : Operator
	{
		public RunCodeBlock ()
			: base( 0, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is CodeBlock )
			{
				return ( First as CodeBlock ).Run();
			}
			throw new Exception( "Operand must be a code block" );
		}
	}
}