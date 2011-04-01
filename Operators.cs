using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

			representationDictionary.Add( typeof( SufixDecrement ), "--" );
			representationDictionary.Add( typeof( PrefixDecrement ), "--" );
			representationDictionary.Add( typeof( SufixIncrement ), "++" );
			representationDictionary.Add( typeof( PrefixIncrement ), "++" );
			representationDictionary.Add( typeof( InvokeOperator ), "invoke!" );
			representationDictionary.Add( typeof( MakeCodeBlock ), "makeCB!" );
			representationDictionary.Add( typeof( AccessValueAtIndex ), "index!" );

			lowestPrecedance = representationDictionary.Keys.Max( x => ( Activator.CreateInstance( x ) as Operator ).Precedance );

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
			var eFirst = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Identifier && eFirst is Number && Second is Number )
			{
				Compiler.SetValue( First as Identifier, new Number( ( eFirst as Number ).Val - ( Second as Number ).Val ) );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class DivisiveAssignment : Operator, IRequiresRuntime
	{
		public DivisiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Identifier && eFirst is Number && Second is Number )
			{
				Compiler.SetValue( First as Identifier, new Number( ( eFirst as Number ).Val / ( Second as Number ).Val ) );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class MultiplicativeAssignment : Operator, IRequiresRuntime
	{
		public MultiplicativeAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Identifier && eFirst is Number && Second is Number )
			{
				Compiler.SetValue( First as Identifier, new Number( ( eFirst as Number ).Val * ( Second as Number ).Val ) );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class AdditiveAssignment : Operator, IRequiresRuntime
	{
		public AdditiveAssignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Identifier && eFirst is Number && Second is Number )
			{
				Compiler.SetValue( First as Identifier, new Number( ( eFirst as Number ).Val + ( Second as Number ).Val ) );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class ModAssignment : Operator, IRequiresRuntime
	{
		public ModAssignment ()
			: base( 14, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Identifier && eFirst is Number && Second is Number )
			{
				Compiler.SetValue( First as Identifier, new Number( ( eFirst as Number ).Val % ( Second as Number ).Val ) );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class Assignment : Operator, IRequiresRuntime
	{
		public Assignment ()
			: base( 15, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			Second = Second.Evaluate();
			if ( First is Identifier )
			{
				Compiler.SetValue( First as Identifier, Second );
				var toReturn = First.Evaluate();
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}

	class Subtraction : Operator
	{
		public Subtraction ()
			: base( 4, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			} else return NoValue.Value;
		}
	}
	class Division : Operator
	{
		public Division ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			} else return NoValue.Value;
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
			if ( First is NoValue || Second is NoValue || First is Expression || Second is Expression || First is ExpressionSequence || Second is ExpressionSequence )
			{
				return NoValue.Value;
			} else if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val + ( Second as Number ).Val );
			} else if ( First is String || Second is String )
			{
				return new String( First.ToString() + Second.ToString() );
			} else return NoValue.Value;
		}
	}
	class Multiplication : Operator
	{
		public Multiplication ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			} else return NoValue.Value;
		}
	}
	class ModOperator : Operator
	{
		public ModOperator ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val % ( Second as Number ).Val );
			} else return NoValue.Value;
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
			var eFirst = First.Evaluate();
			var eSecond = Second.Evaluate().ToArray();
			if ( eFirst is Function )
			{
				var toReturn = ( eFirst as Function ).Invoke( eSecond );
				Compiler.ExitInstanceScope();
				return toReturn;
			} else return NoValue.Value;
		}
	}
	class MakeCodeBlock : Operator
	{
		public MakeCodeBlock ()
			: base( 13, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Expression )
			{
				return new CodeBlock( ( First as Expression ) );
			} else if ( First is ExpressionSequence )
			{
				return new CodeBlock( ( First as ExpressionSequence ).Tokenize() );
			} else return NoValue.Value;
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
			if ( First is Array && Second is Number )
			{
				int index = (int)( ( Second as Number ).Val );
				if ( index >= 0 && index < ( First as Array ).Arr.Count )
				{
					return ( First as Array ).Arr[ index ].Evaluate();
				} else throw new Exception( "Index out of range" );
			} else return NoValue.Value;
		}
	}

	class NotOperator : Operator
	{
		public NotOperator ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean )
			{
				return new Boolean( ( First as Boolean ).Val ? false : true );
			}
			return NoValue.Value;
		}
	}

	class NotEqualTo : Operator
	{
		public NotEqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val != ( Second as Number ).Val );
				else if ( First is String ) return new Boolean( ( First as String ).Val != ( Second as String ).Val );
				else if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val != ( Second as Boolean ).Val );
				else return NoValue.Value;
			} else return new Boolean( true );
		}
	}
	class EqualTo : Operator
	{
		public EqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( !( First is NoValue || Second is NoValue ) && First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val == ( Second as Number ).Val );
				else if ( First is String ) return new Boolean( ( First as String ).Val == ( Second as String ).Val );
				else if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val == ( Second as Boolean ).Val );
				else return NoValue.Value;
			} else return NoValue.Value;
		}
	}
	class LessThan : Operator
	{
		public LessThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val < ( Second as Number ).Val );
			else return NoValue.Value;
		}
	}
	class LessOrEqual : Operator
	{
		public LessOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val <= ( Second as Number ).Val );
			else return NoValue.Value;
		}
	}
	class GreaterThan : Operator
	{
		public GreaterThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val > ( Second as Number ).Val );
			else return NoValue.Value;
		}
	}
	class GreaterOrEqual : Operator
	{
		public GreaterOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val >= ( Second as Number ).Val );
			else return NoValue.Value;
		}
	}

	class LogicalOr : Operator
	{
		public LogicalOr ()
			: base( 12, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val || ( Second as Boolean ).Val );
			else return NoValue.Value;
		}
	}
	class LogicalAnd : Operator
	{
		public LogicalAnd ()
			: base( 11, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val && ( Second as Boolean ).Val );
			else return NoValue.Value;
		}
	}

	class SufixIncrement : Operator, IRequiresRuntime
	{
		public SufixIncrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			if ( First is Identifier && eFirst is Number )
			{
				Compiler.SetValue( ( First as Identifier ), new Number( ( eFirst as Number ).Val + 1 ) );
				return First.Evaluate();
			} else return NoValue.Value;
		}
	}
	class SufixDecrement : Operator, IRequiresRuntime
	{
		public SufixDecrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			if ( First is Identifier && eFirst is Number )
			{
				Compiler.SetValue( ( First as Identifier ), new Number( ( eFirst as Number ).Val - 1 ) );
				return First.Evaluate();
			} else return NoValue.Value;
		}
	}
	class PrefixIncrement : Operator, IRequiresRuntime
	{
		public PrefixIncrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			if ( First is Identifier && eFirst is Number )
			{
				Compiler.SetValue( ( First as Identifier ), new Number( ( eFirst as Number ).Val + 1 ) );
				return First.Evaluate();
			} else return NoValue.Value;
		}
	}
	class PrefixDecrement : Operator, IRequiresRuntime
	{
		public PrefixDecrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			var eFirst = First.Evaluate();
			if ( First is Identifier && eFirst is Number )
			{
				Compiler.SetValue( ( First as Identifier ), new Number( ( eFirst as Number ).Val - 1 ) );
				return First.Evaluate();
			} else return NoValue.Value;
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
			if ( First is Boolean && Second is CodeBlock )
			{
				if ( ( First as Boolean ).Val ) return ( Second as CodeBlock ).Run();
				else return new ConditionNotMet();
			} else return NoValue.Value;
		}
	}
	class ElseOperator : Operator, IRequiresRuntime
	{
		public ElseOperator ()
			: base( 16, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is ConditionNotMet ) return ( Second as CodeBlock ).Run();
			else return NoValue.Value;
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
			if ( First is CodeBlock && Second is CodeBlock )
			{
				CodeBlock block = ( Second as CodeBlock );
				block.Value.AddRange( ( First as CodeBlock ).Value );
				block.Value.Add( new CodeBlock( new Token[] { new ContinueOperator() }.ToList() ) );
				block.Value.Add( new IfOperator() );
				block.Value.Add( new CodeBlock( new Token[] { new BreakOperator() }.ToList() ) );
				block.Value.Add( new ElseOperator() );
				return new Loop( block );
			} else return NoValue.Value;
		}
	}
	class FunctionOperator : Operator
	{
		public FunctionOperator ()
			: base( 14, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			Second = Second.Evaluate();
			Array arr;
			CodeBlock code;
			if ( First is String ) arr = First.ToArray();
			else if ( First is Array ) arr = ( First as Array );
			else return NoValue.Value;
			if ( Second is CodeBlock ) code = ( Second as CodeBlock );
			else return NoValue.Value;

			return new Function( arr, code );
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
			if ( First is Array && ( First as Array ).Arr.Count == 0 && Second is CodeBlock )
			{
				var newType = new Type( ( Second as CodeBlock ) );
				return newType.Run();
			} else if ( First is Type && Second is CodeBlock )
			{
				var newType = new Type( ( First as Type ), ( Second as CodeBlock ) );
				return newType.Run();
			} else return NoValue.Value;
		}
	}
	class NewOperator : Operator, IRequiresRuntime
	{
		public NewOperator ()
			: base( 4, OperatorType.PrefixUnary ) { }
		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Type )
			{
				return new Instance( ( First as Type ) );
			} else return NoValue.Value;
		}
	}

	class CommaOperator : Operator, IRequiresRuntime
	{
		public CommaOperator ()
			: base( 13, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Array )
			{
				( First as Array ).Arr.Add( Second );
				return First;
			} else return new Array( First, Second );
		}
	}
	class DotOperator : Operator, IRequiresRuntime
	{
		public DotOperator ()
			: base( 1, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			First = First.Evaluate();
			if ( First is Instance && Second is Identifier )
			{
				Compiler.SetAsCurrentScope( ( First as Instance ).Identifiers );
				Compiler.InInstanceScope = true;
				return Second;
			} else return NoValue.Value;
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
			if ( First is CodeBlock )
			{
				return ( First as CodeBlock ).Run();
			} else return NoValue.Value;
		}
	}
}
