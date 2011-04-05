using System;
using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	enum OperatorType : int
	{
		PrefixUnary = 0,
		SufixUnary,
		InfixBinary,
		PrefixBinary,
		SufixBinary,
		Special
	}
	abstract class Operator : Token
	{
		private int precedance;
		public int Precedance
		{
			get { return precedance; }
			set { precedance = value; }
		}

		public override string ToString ()
		{
			return Operators.RepresentationDictionary[ this.GetType() ];
		}

		protected OperatorType type;
		public OperatorType Type
		{
			get { return type; }
			set { type = value; }
		}

		public Operator ( int Precedance, OperatorType Type )
		{
			precedance = Precedance;
			type = Type;
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
		static int lowestPrecedance;
		public static int LowestPrecedance
		{
			get { return Operators.lowestPrecedance; }
			set { Operators.lowestPrecedance = value; }
		}

		static Dictionary<string, System.Type> operatorDictionary;
		public static Dictionary<string, System.Type> OperatorDictionary
		{
			get { return Operators.operatorDictionary; }
			set { Operators.operatorDictionary = value; }
		}

		static Dictionary<System.Type, string> representationDictionary;
		public static Dictionary<System.Type, string> RepresentationDictionary
		{
			get { return Operators.representationDictionary; }
			set { Operators.representationDictionary = value; }
		}

		static LinkedList<BracketType> brackets;
		public static LinkedList<BracketType> Brackets
		{
			get { return Operators.brackets; }
			set { Operators.brackets = value; }
		}

		static Operators ()
		{
			operatorDictionary = new Dictionary<string, System.Type>();
			representationDictionary = new Dictionary<System.Type, string>();
			brackets = new LinkedList<BracketType>();

			add( "-=", typeof( SubtractiveAssignment ) );
			add( "-", typeof( Subtraction ) );
			add( "(", typeof( ParenthesisOpen ) );
			add( ")", typeof( ParenthesisClosed ) );
			add( "!", typeof( NotOperator ) );
			add( "!=", typeof( NotEqualTo ) );
			add( "*=", typeof( MultiplicativeAssignment ) );
			add( "*", typeof( Multiplication ) );
			add( "%", typeof( ModOperator ) );
			add( "%=", typeof( ModAssignment ) );
			add( "||", typeof( LogicalOr ) );
			add( "&&", typeof( LogicalAnd ) );
			add( "<", typeof( LessThan ) );
			add( "<=", typeof( LessOrEqual ) );
			add( ">", typeof( GreaterThan ) );
			add( ">=", typeof( GreaterOrEqual ) );
			add( "==", typeof( EqualTo ) );
			add( "+=", typeof( AdditiveAssignment ) );
			add( "=", typeof( Assignment ) );
			add( "/", typeof( Division ) );
			add( "/=", typeof( DivisiveAssignment ) );
			add( "+", typeof( Addition ) );
			add( ".", typeof( DotOperator ) );
			add( "?", typeof( Unidentified ) );
			add( "if", typeof( IfOperator ) );
			add( "else", typeof( ElseOperator ) );
			add( "{", typeof( CurlyBracesOpen ) );
			add( "}", typeof( CurlyBracesClosed ) );
			add( ",", typeof( CommaOperator ) );
			add( "function", typeof( FunctionOperator ) );
			add( "return", typeof( ReturnOperator ) );
			add( "continue", typeof( ContinueOperator ) );
			add( "break", typeof( BreakOperator ) );
			add( "while", typeof( WhileOperator ) );
			add( "[", typeof( SquareBracketsOpen ) );
			add( "]", typeof( SquareBracketsClosed ) );
			add( "class", typeof( ClassOperator ) );
			add( "new", typeof( NewOperator ) );
			add( "&", typeof( ReferenceOperator ) );
			add( "&=", typeof( ReferenceAssignment ) );
			add( "typeof", typeof( TypeofOperator ) );
			add( "declare", typeof( DeclareOperator ) );

			representationDictionary.Add( typeof( SufixDecrement ), "--" );
			representationDictionary.Add( typeof( PrefixDecrement ), "--" );
			representationDictionary.Add( typeof( SufixIncrement ), "++" );
			representationDictionary.Add( typeof( PrefixIncrement ), "++" );
			representationDictionary.Add( typeof( InvokeOperator ), "invoke!" );
			representationDictionary.Add( typeof( MakeCodeBlock ), "makeCB!" );
			representationDictionary.Add( typeof( AccessValueAtIndex ), "index!" );

			lowestPrecedance = representationDictionary.Keys.Max( X => ( (Operator)Activator.CreateInstance( X ) ).Precedance );

			brackets.AddLast( new BracketType( typeof( CurlyBracesOpen ), typeof( CurlyBracesClosed ), typeof( MakeCodeBlock ) ) );
			brackets.AddLast( new BracketType( typeof( SquareBracketsOpen ), typeof( SquareBracketsClosed ), typeof( AccessValueAtIndex ) ) );
		}
		static void add ( string Rep, System.Type Type )
		{
			operatorDictionary.Add( Rep, Type );
			representationDictionary.Add( Type, Rep );
		}
	}

	class SubtractiveAssignment : Operator, IRequiresRuntime
	{
		public SubtractiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class DivisiveAssignment : Operator, IRequiresRuntime
	{
		public DivisiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class MultiplicativeAssignment : Operator, IRequiresRuntime
	{
		public MultiplicativeAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class AdditiveAssignment : Operator, IRequiresRuntime
	{
		public AdditiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class ModAssignment : Operator, IRequiresRuntime
	{
		public ModAssignment ()
			: base( 14, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class ReferenceAssignment : Operator, IRequiresRuntime
	{
		public ReferenceAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();

			if ( First is Reference && Second is Reference )
			{
				( First as Reference ).ChangeReference( Second as Reference );
				return Second;
			}
			throw new Exception( "Both operands must be identifiers" );
		}
	}
	class Assignment : Operator, IRequiresRuntime
	{
		public Assignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			bool clone = !( Second is HardReference );
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Reference )
			{
				var toSet = clone ? Second.Clone() : Second;
				( First as Reference ).ChangeReference( Compiler.StoreValue( toSet ) );
				return toSet;
			}
			throw new Exception( "Operands must be an identifier and a value" );
		}
	}

	class Subtraction : Operator
	{
		public Subtraction ()
			: base( 4, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			if ( !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}
	class Division : Operator
	{
		public Division ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			if ( !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}
	class Addition : Operator
	{
		public Addition ()
			: base( 4, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is NoValue || Second is NoValue
				|| First is Expression || Second is Expression
				|| First is ExpressionSequence || Second is ExpressionSequence
				|| First is CodeBlock || Second is CodeBlock )
			{
				if ( Compiler.Runtime )
				{
					throw new Exception( "One of the operands is of wrong type" );
				}
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
			throw new Exception( "Both operands must be numbers or one of the operands must be a string" );
		}
	}
	class Multiplication : Operator
	{
		public Multiplication ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			if ( !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
		}
	}
	class ModOperator : Operator
	{
		public ModOperator ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Number( (int)( First as Number ).Val % (int)( Second as Number ).Val );
			if ( !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val % ( Second as Number ).Val );
			}
			throw new Exception( "Both operands must be numbers" );
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

	class InvokeOperator : Operator, IRequiresRuntime
	{
		public InvokeOperator ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;
			Second = Second.ToArray();

			if ( First is Function )
			{
				var toReturn = ( First as Function ).Invoke( ( Second.Clone() as Array ) );
				return toReturn;
			}
			throw new Exception( "First operand must be a function" );
		}
	}
	class MakeCodeBlock : Operator
	{
		public MakeCodeBlock ()
			: base( 13, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Identifier && !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
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
	class AccessValueAtIndex : Operator, IRequiresRuntime
	{
		public AccessValueAtIndex ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Array && Second is Number )
			{
				int index = (int)( Second as Number ).Val;
				if ( index >= 0 && index < ( First as Array ).Arr.Count )
				{
					var toReturn = ( First as Array ).Arr[ index ];
					return toReturn;
				}
				throw new Exception( "Index is out of bounds of the array" );
			}
			throw new Exception( "First operand must be an array and the second one must be a number" );
		}
	}

	class NotOperator : Operator
	{
		public NotOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Identifier && !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			throw new Exception( "Operand must be a boolean" );
		}
	}
	class ReferenceOperator : Operator, IRequiresRuntime
	{
		public ReferenceOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();

			if ( First is Reference )
			{
				return ( First as Reference ).GetHardReference();
			}
			throw new Exception( "Reference operator expects a identifier" );
		}
	}
	class TypeofOperator : Operator, IRequiresRuntime
	{
		public TypeofOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			return new String( First.GetType().ToString() );
		}
	}

	class NotEqualTo : Operator
	{
		public NotEqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( ( First is Identifier || Second is Identifier ) && !Compiler.Runtime ) return NoValue.Value;
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val != ( Second as Number ).Val );
				if ( First is String ) return new Boolean( ( First as String ).Val != ( Second as String ).Val );
				if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val != ( Second as Boolean ).Val );
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
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( !( First is NoValue || Second is NoValue ) && First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val == ( Second as Number ).Val );
				if ( First is String ) return new Boolean( ( First as String ).Val == ( Second as String ).Val );
				if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val == ( Second as Boolean ).Val );
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
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val < ( Second as Number ).Val );
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
			First = First.Evaluate();
			Second = Second.Evaluate();
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
			First = First.Evaluate();
			Second = Second.Evaluate();
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
			First = First.Evaluate();
			Second = Second.Evaluate();
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
			First = First.Evaluate();
			Second = Second.Evaluate();
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
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val && ( Second as Boolean ).Val );
			throw new Exception( "Operands must be boolean" );
		}
	}

	class SufixIncrement : Operator, IRequiresRuntime
	{
		public SufixIncrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();

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
	class SufixDecrement : Operator, IRequiresRuntime
	{
		public SufixDecrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();

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
	class PrefixIncrement : Operator, IRequiresRuntime
	{
		public PrefixIncrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();

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
	class PrefixDecrement : Operator, IRequiresRuntime
	{
		public PrefixDecrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();

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

	class IfOperator : Operator, IRequiresRuntime
	{
		public IfOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Boolean && Second is CodeBlock )
			{
				if ( ( First as Boolean ).Val ) return ( Second as CodeBlock ).Run();
				return new ConditionNotMet();
			}
			throw new Exception( "Operands must be a boolean and a code block" );
		}
	}
	class ElseOperator : Operator, IRequiresRuntime
	{
		public ElseOperator ()
			: base( 16, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class WhileOperator : Operator
	{
		public WhileOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Expression ) First = new CodeBlock( First as Expression );
			if ( First is CodeBlock && Second is CodeBlock )
			{
				var block = ( Second as CodeBlock );
				block.Value.AddRange( ( First as CodeBlock ).Value );
				block.Value.Add( new CodeBlock( new Token[] { new ContinueOperator() }.ToList() ) );
				block.Value.Add( new IfOperator() );
				block.Value.Add( new CodeBlock( new Token[] { new BreakOperator() }.ToList() ) );
				block.Value.Add( new ElseOperator() );
				return new Loop( block );
			}
			throw new Exception( "Operands must be code blocks" );
		}
	}

	class FunctionOperator : Operator, IRequiresRuntime
	{
		public FunctionOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			Array arr = First.ToArray();
			CodeBlock code;

			if ( Second is CodeBlock )
			{
				code = ( Second as CodeBlock );
				return new Function( arr, code, Compiler.GetCurrentScope() );
			}
			throw new Exception( "Function must be followed by a list and a code block" );

		}
	}
	class ClassOperator : Operator, IRequiresRuntime
	{
		public ClassOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;
			if ( Second is Reference ) Second = ( Second as Reference ).ReferencingValue;

			if ( First is Array && ( First as Array ).Arr.Count == 0 && Second is CodeBlock )
			{
				var newType = new Type( ( Second as CodeBlock ) );
				return newType.Run();
			}
			if ( First is Type && Second is CodeBlock )
			{
				var newType = new Type( ( First as Type ), ( Second as CodeBlock ) );
				return newType.Run();
			}
			throw new Exception( "First operand must be a type or an empty list and the second operand must bea code block" );
		}
	}
	class DeclareOperator : Operator, IRequiresRuntime
	{
		public DeclareOperator ()
			: base( 14, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Identifier )
			{
				return ( First as Identifier ).Evaluate();
			}
			throw new Exception( "Operand must be an identifier" );
		}
	}
	class NewOperator : Operator, IRequiresRuntime
	{
		public NewOperator ()
			: base( 4, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is Type )
			{
				return new Instance( ( First as Type ) );
			}
			throw new Exception( "Operand must be a type" );
		}
	}

	class CommaOperator : Operator, IRequiresRuntime
	{
		public CommaOperator ()
			: base( 13, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			bool add = !( First is Identifier );
			First = First.Evaluate();
			Second = Second.Evaluate();
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
	class DotOperator : Operator, IRequiresRuntime
	{
		public DotOperator ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is Instance && Second is Identifier )
			{
				Instance inst = First as Instance;
				Identifier ident = Second as Identifier;
				if ( inst.Identifiers.ContainsKey( ident.Name ) )
				{
					Compiler.SetAsCurrentScope( inst.Identifiers );
					Second = Second.Evaluate();
					Compiler.ExitScope( true );
					return Second as Reference;
				}
				throw new Exception( "Instance does not hold that identifiers" );
			}
			throw new Exception( "Operands must be an instance and an identifier" );
		}
	}

	class ReturnOperator : Operator, IRequiresRuntime
	{
		public ReturnOperator ()
			: base( 15, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class ContinueOperator : Operator, IRequiresRuntime
	{
		public ContinueOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}
	class BreakOperator : Operator, IRequiresRuntime
	{
		public BreakOperator ()
			: base( 15, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return NoValue.Value;
		}
	}

	class RunCodeBlock : Operator, IRequiresRuntime
	{
		public RunCodeBlock ()
			: base( 0, OperatorType.SufixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Reference ) First = ( First as Reference ).ReferencingValue;

			if ( First is CodeBlock )
			{
				return ( First as CodeBlock ).Run();
			}
			throw new Exception( "Operand must be a code block" );
		}
	}
}
