using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable RedundantUsingDirective
using Kento.Utility;
// ReSharper restore RedundantUsingDirective

namespace Kento
{
	abstract class Operator : Token
	{
		public int Precedance { get; set; }

		public override string ToString ()
		{
			return Operators.RepresentationDictionary[ GetType() ];
		}

		public OperatorType Type { get; set; }

		protected Operator ( int Precedance, OperatorType Type )
		{
			this.Precedance = Precedance;
			this.Type = Type;
		}
		public abstract Value Operate ( Value First, Value Second );
	}
	struct BracketType
	{
		System.Type openingBracket, closingBracket, specialOperator;
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

		public BracketType ( System.Type Open, System.Type Closed, System.Type Special )
		{
			openingBracket = Open;
			closingBracket = Closed;
			specialOperator = Special;
		}
	}
	class Operators
	{
		public static int LowestPrecedance { get; set; }

		static Dictionary<string, System.Type> operatorDictionary;
		public static Dictionary<string, System.Type> OperatorDictionary
		{
			get { return operatorDictionary; }
			set { operatorDictionary = value; }
		}

		static Dictionary<System.Type, string> representationDictionary;
		public static Dictionary<System.Type, string> RepresentationDictionary
		{
			get { return representationDictionary; }
			set { representationDictionary = value; }
		}

		static LinkedList<BracketType> brackets;
		public static LinkedList<BracketType> Brackets
		{
			get { return brackets; }
			set { brackets = value; }
		}

		static Operators ()
		{
			operatorDictionary = new Dictionary<string, System.Type>();
			representationDictionary = new Dictionary<System.Type, string>();
			brackets = new LinkedList<BracketType>();

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

			representationDictionary.Add( typeof( SufixDecrement ), "--" );
			representationDictionary.Add( typeof( PrefixDecrement ), "--" );
			representationDictionary.Add( typeof( SufixIncrement ), "++" );
			representationDictionary.Add( typeof( PrefixIncrement ), "++" );
			representationDictionary.Add( typeof( InvokeOperator ), "invoke!" );
			representationDictionary.Add( typeof( MakeCodeBlock ), "makeCB!" );
			representationDictionary.Add( typeof( AccessValueAtIndex ), "index!" );

			LowestPrecedance = representationDictionary.Keys.Max( X => ( (Operator)Activator.CreateInstance( X ) ).Precedance );

			brackets.AddLast( new BracketType( typeof( CurlyBracesOpen ), typeof( CurlyBracesClosed ), typeof( MakeCodeBlock ) ) );
			brackets.AddLast( new BracketType( typeof( SquareBracketsOpen ), typeof( SquareBracketsClosed ), typeof( AccessValueAtIndex ) ) );
		}
		static void Add ( string Rep, System.Type Type )
		{
			operatorDictionary.Add( Rep, Type );
			representationDictionary.Add( Type, Rep );
		}
	}

	class SubtractiveAssignment : Operator
	{
		public SubtractiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				var value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val - ( value2 as Number ).Val );
					var toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}
	class DivisiveAssignment : Operator
	{
		public DivisiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				var value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val / ( value2 as Number ).Val );
					var toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}
	class MultiplicativeAssignment : Operator
	{
		public MultiplicativeAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				var value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val * ( value2 as Number ).Val );
					var toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}
	class AdditiveAssignment : Operator
	{
		public AdditiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				var value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val + ( value2 as Number ).Val );
					var toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}
	class ModAssignment : Operator
	{
		public ModAssignment ()
			: base( 14, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				var value1 = ( First as Reference ).ReferencingValue;

				Value value2;
				if ( Second is Reference ) value2 = ( Second as Reference ).ReferencingValue;
				else value2 = Second;

				if ( value1 is Number && value2 is Number )
				{
					( First as Reference ).ReferencingValue = new Number( ( value1 as Number ).Val % ( value2 as Number ).Val );
					var toReturn = ( First as Reference ).ReferencingValue;
					return toReturn;
				}
				throw new Exception( "Operands need to be numbers" );
			}
			throw new Exception( "Assignment needs an identifier" );
		}
	}
	class ReferenceAssignment : Operator
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
	class Assignment : Operator
	{
		public Assignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			bool clone = Second is Reference && !( Second is HardReference );
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Reference )
			{
				var toSet = clone ? Second.Clone() : Second;
				( First as Reference ).ReferencingValue = toSet;
				return toSet;
			}
			throw new Exception( "Operands must be an identifier and a value" );
		}
	}

	class Subtraction : Operator, ICanRunAtCompile
	{
		public Subtraction ()
			: base( 4, OperatorType.InfixBinary ) { }

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
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			return NoValue.Value;
		}
	}
	class Division : Operator, ICanRunAtCompile
	{
		public Division ()
			: base( 3, OperatorType.InfixBinary ) { }

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
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			return NoValue.Value;
		}
	}
	class Addition : Operator, ICanRunAtCompile
	{
		public Addition ()
			: base( 4, OperatorType.InfixBinary ) { }

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
	}
	class Multiplication : Operator, ICanRunAtCompile
	{
		public Multiplication ()
			: base( 3, OperatorType.InfixBinary ) { }

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
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			return NoValue.Value;
		}
	}
	class ModOperator : Operator, ICanRunAtCompile
	{
		public ModOperator ()
			: base( 3, OperatorType.InfixBinary ) { }

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
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( (int)( First as Number ).Val % (int)( Second as Number ).Val );
			return NoValue.Value;
		}
	}

	class ParenthesisOpen : Operator
	{
		public ParenthesisOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class ParenthesisClosed : Operator
	{
		public ParenthesisClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class CurlyBracesOpen : Operator
	{
		public CurlyBracesOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class CurlyBracesClosed : Operator
	{
		public CurlyBracesClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class SquareBracketsOpen : Operator
	{
		public SquareBracketsOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class SquareBracketsClosed : Operator
	{
		public SquareBracketsClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	class InvokeOperator : Operator
	{
		public InvokeOperator ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference && !( Second is HardReference ) ) Second = ( Second as Reference ).ReferencingValue;
			Second = Second.ToArray();

			if ( First is IFunction )
			{
				var toReturn = ( First as IFunction ).Invoke( ( Second.Clone() as Array ) );
				return toReturn;
			}
			throw new Exception( "First operand must be a function" );
		}
	}
	class MakeCodeBlock : Operator, ICanRunAtCompile
	{
		public MakeCodeBlock ()
			: base( 13, OperatorType.PrefixUnary ) { }

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
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			return Operate( First, Second );
		}
	}
	class AccessValueAtIndex : Operator
	{
		public AccessValueAtIndex ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is IIndexable && Second is Number )
			{
				var array = First as IIndexable;
				var index = (int)( Second as Number ).Val;
				var toReturn = array.GetReferenceAtIndex( index );
				return toReturn;
			}
			throw new Exception( "First operand must be indexable and the second one must be a number" );
		}
	}

	class NotOperator : Operator, ICanRunAtCompile
	{
		public NotOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			throw new Exception( "Operand must be a boolean" );
		}
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			return NoValue.Value;
		}
	}
	class ReferenceOperator : Operator
	{
		public ReferenceOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				return ( First as Reference ).GetHardReference();
			}
			throw new Exception( "Reference operator expects an identifier" );
		}
	}
	class TypeofOperator : Operator
	{
		public TypeofOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			return new String( First.GetType().ToString() );
		}
	}
	class StaticOperator : Operator
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

	class NotEqualTo : Operator
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
	class EqualTo : Operator
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
	class LessThan : Operator
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
	class LessOrEqual : Operator
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
	class GreaterThan : Operator
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
	class GreaterOrEqual : Operator
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

	class LogicalOr : Operator
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
	class LogicalAnd : Operator
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

	class SufixIncrement : Operator
	{
		public SufixIncrement ()
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
	class SufixDecrement : Operator
	{
		public SufixDecrement ()
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
	class PrefixIncrement : Operator
	{
		public PrefixIncrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				if ( ( First as Reference ).ReferencingValue is Number )
				{
					var toReturn = new Number( ( ( First as Reference ).ReferencingValue as Number ).Val + 1 );
					( First as Reference ).ReferencingValue = toReturn;
					return toReturn;
				}
				throw new Exception( "Operand must be a number" );
			}
			throw new Exception( "Operand must be an identifier" );
		}
	}
	class PrefixDecrement : Operator
	{
		public PrefixDecrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference )
			{
				if ( ( First as Reference ).ReferencingValue is Number )
				{
					var toReturn = new Number( ( ( First as Reference ).ReferencingValue as Number ).Val - 1 );
					( First as Reference ).ReferencingValue = toReturn;
					return toReturn;
				}
				throw new Exception( "Operand must be a number" );
			}
			throw new Exception( "Operand must be an identifier" );
		}
	}

	class Unidentified : Operator
	{
		public Unidentified ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	class IfOperator : Operator
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
					var toReturn = ( Second as CodeBlock ).Run();
					return toReturn;
				}
				return new ConditionNotMet();
			}
			throw new Exception( "Operands must be a boolean and a code block" );
		}
	}
	class ElseOperator : Operator
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
	class WhileOperator : Operator, ICanRunAtCompile
	{
		public WhileOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Expression ) First = new CodeBlock( First as Expression );
			if ( First is CodeBlock && Second is CodeBlock )
			{
				return new Loop( First as CodeBlock, Second as CodeBlock );
			}
			throw new Exception( "Operands must be code blocks" );
		}
		public Value CompileTimeOperate ( Value First, Value Second )
		{
			if ( First is Expression ) First = new CodeBlock( First as Expression );
			if ( First is CodeBlock && Second is CodeBlock )
			{
				return new Loop( First as CodeBlock, Second as CodeBlock );
			}
			return NoValue.Value;
		}
	}

	class FunctionOperator : Operator
	{
		public FunctionOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			var arr = First.ToArray();

			if ( Second is CodeBlock )
			{
				var code = ( Second as CodeBlock );
				return new Function( arr, code, Compiler.GetCurrentScope() );
			}
			throw new Exception( "Function must be followed by a list and a code block" );
		}
	}
	class ClassOperator : Operator
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
	class DeclareOperator : Operator
	{
		public DeclareOperator ()
			: base( 14, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return First.Evaluate();
		}
	}
	class NewOperator : Operator
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

	class CommaOperator : Operator
	{
		public CommaOperator ()
			: base( 13, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			bool add = !( First is Identifier );
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is Array && add )
			{
				if ( Second is Reference )
				{
					( First as Array ).Arr.Add( Second as Reference );
				} else
					( First as Array ).Arr.Add( new Reference( Second ) );
				return First;
			}
			return new Array( new Reference( First ), ( Second is Reference ) ?
				( Second as Reference ) :
				( new Reference( Second ) ) );
		}
	}
	class DotOperator : Operator
	{
		public DotOperator ()
			: base( 1, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is IHasMembers )
			{
				var inst = First as IHasMembers;
				Compiler.SetAsCurrentScope( inst.Identifiers );
				Compiler.PendingDot = true;
				Compiler.PendingDotIsStatic = First is IClass;
				return NoValue.Value;
			}
			throw new Exception( "Operand must have members" );
		}
	}

	class ReturnOperator : Operator
	{
		public ReturnOperator ()
			: base( 15, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class ContinueOperator : Operator
	{
		public ContinueOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class BreakOperator : Operator
	{
		public BreakOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	class RunCodeBlock : Operator
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
