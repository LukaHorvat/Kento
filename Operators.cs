﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

			representationDictionary.Add( typeof( SufixDecrement ), "--" );
			representationDictionary.Add( typeof( PrefixDecrement ), "--" );
			representationDictionary.Add( typeof( SufixIncrement ), "++" );
			representationDictionary.Add( typeof( PrefixIncrement ), "++" );
			representationDictionary.Add( typeof( InvokeOperator ), "()" );
			representationDictionary.Add( typeof( MakeCodeBlock ), "{}" );

			lowestPrecedance = representationDictionary.Keys.Max( x => ( Activator.CreateInstance( x ) as Operator ).Precedance );

			brackets.AddLast( new BracketType( typeof( CurlyBracesOpen ), typeof( CurlyBracesClosed ), typeof( MakeCodeBlock ) ) );
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
			if ( First is Number && Second is Number )
			{
				( First as Number ).Val -= ( Second as Number ).Val;
				return First.Evaluate();
			} else return new NoValue();
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
			if ( First is Number && Second is Number )
			{
				( First as Number ).Val /= ( Second as Number ).Val;
				return First.Evaluate();
			} else return new NoValue();
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
			if ( First is Number && Second is Number )
			{
				( First as Number ).Val *= ( Second as Number ).Val;
				return First.Evaluate();
			} else return new NoValue();
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
			if ( First is Number && Second is Number )
			{
				( First as Number ).Val += ( Second as Number ).Val;
				return First.Evaluate();
			} else return new NoValue();
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
			if ( First is Number && Second is Number )
			{
				( First as Number ).Val %= ( Second as Number ).Val;
				return First.Evaluate();
			} else return new NoValue();
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
				Compiler.ExecutingScope.Identifiers[ ( First as Identifier ).Name ] = Second;
				return First.Evaluate();
			} else return new NoValue();
		}
	}

	class Subtraction : Operator
	{
		public Subtraction ()
			: base( 4, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val - ( Second as Number ).Val );
			} else return new NoValue();
		}
	}
	class Division : Operator
	{
		public Division ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val / ( Second as Number ).Val );
			} else return new NoValue();
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
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val + ( Second as Number ).Val );
			} else if ( First is String )
			{
				return new String( First.ToString() + Second.ToString() );
			} else return new NoValue();
		}
	}
	class Multiplication : Operator
	{
		public Multiplication ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val * ( Second as Number ).Val );
			} else return new NoValue();
		}
	}
	class ModOperator : Operator
	{
		public ModOperator ()
			: base( 3, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number )
			{
				return new Number( ( First as Number ).Val % ( Second as Number ).Val );
			} else return new NoValue();
		}
	}

	class ParenthesisOpen : Operator
	{
		public ParenthesisOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class ParenthesisClosed : Operator
	{
		public ParenthesisClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class CurlyBracesOpen : Operator
	{
		public CurlyBracesOpen ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class CurlyBracesClosed : Operator
	{
		public CurlyBracesClosed ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
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
			var test = Compiler.Runtime;
			if ( eFirst is Function )
			{
				return ( eFirst as Function ).Invoke( eSecond );
			} else return new NoValue();
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
				return new CodeBlock( (First as Expression) );
			} else return new NoValue();
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
			return new NoValue();
		}
	}

	class NotEqualTo : Operator
	{
		public NotEqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val != ( Second as Number ).Val );
				else if ( First is String ) return new Boolean( ( First as String ).Val != ( Second as String ).Val );
				else if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val != ( Second as Boolean ).Val );
				else return new NoValue();
			} else return new Boolean( true );
		}
	}
	class EqualTo : Operator
	{
		public EqualTo ()
			: base( 7, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First.GetType() == Second.GetType() )
			{
				if ( First is Number ) return new Boolean( ( First as Number ).Val == ( Second as Number ).Val );
				else if ( First is String ) return new Boolean( ( First as String ).Val == ( Second as String ).Val );
				else if ( First is Boolean ) return new Boolean( ( First as Boolean ).Val == ( Second as Boolean ).Val );
				else return new NoValue();
			} else return new Boolean( false );
		}
	}
	class LessThan : Operator
	{
		public LessThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val < ( Second as Number ).Val );
			else return new NoValue();
		}
	}
	class LessOrEqual : Operator
	{
		public LessOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val <= ( Second as Number ).Val );
			else return new NoValue();
		}
	}
	class GreaterThan : Operator
	{
		public GreaterThan ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val > ( Second as Number ).Val );
			else return new NoValue();
		}
	}
	class GreaterOrEqual : Operator
	{
		public GreaterOrEqual ()
			: base( 6, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Number && Second is Number ) return new Boolean( ( First as Number ).Val >= ( Second as Number ).Val );
			else return new NoValue();
		}
	}

	class LogicalOr : Operator
	{
		public LogicalOr ()
			: base( 12, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val || ( Second as Boolean ).Val );
			else return new NoValue();
		}
	}
	class LogicalAnd : Operator
	{
		public LogicalAnd ()
			: base( 11, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean && Second is Boolean ) return new Boolean( ( First as Boolean ).Val && ( Second as Boolean ).Val );
			else return new NoValue();
		}
	}

	class SufixIncrement : Operator, IRequiresRuntime
	{
		public SufixIncrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class SufixDecrement : Operator, IRequiresRuntime
	{
		public SufixDecrement ()
			: base( 2, OperatorType.SufixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class PrefixIncrement : Operator, IRequiresRuntime
	{
		public PrefixIncrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}
	class PrefixDecrement : Operator, IRequiresRuntime
	{
		public PrefixDecrement ()
			: base( 2, OperatorType.PrefixUnary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}

	class Unidentified : Operator
	{
		public Unidentified ()
			: base( 0, OperatorType.Special ) { }

		public override Value Operate ( Value First, Value Second )
		{
			return new NoValue();
		}
	}

	class IfOperator : Operator
	{
		public IfOperator ()
			: base( 15, OperatorType.PrefixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is Boolean && ( Second is CodeBlock || Second is Expression ) )
			{
				if ( ( First as Boolean ).Val ) return Second;
				else return new ConditionNotMet();
			} else return new NoValue();
		}
	}
	class ElseOperator : Operator
	{
		public ElseOperator ()
			: base( 16, OperatorType.InfixBinary ) { }

		public override Value Operate ( Value First, Value Second )
		{
			if ( First is ConditionNotMet ) return Second;
			else return new NoValue();
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
			Array arr;
			CodeBlock code;
			if ( First is String ) arr = First.ToArray();
			else if ( First is Array ) arr = ( First as Array );
			else return new NoValue();
			if ( Second is CodeBlock ) code = ( Second as CodeBlock );
			else return new NoValue();

			return new Function( arr, code );
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
			return new NoValue();
		}
	}
}