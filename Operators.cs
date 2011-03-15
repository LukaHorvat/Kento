using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Operators
	{
		static Dictionary<string, Type> operatorDictionary;
		public static Dictionary<string, Type> OperatorDictionary
		{
			get { return Operators.operatorDictionary; }
			set { Operators.operatorDictionary = value; }
		}

		static Operators ()
		{
			operatorDictionary = new Dictionary<string, Type>();
			operatorDictionary.Add( "-=", typeof( SubtractiveAssignment ) );
			operatorDictionary.Add( "-", typeof( Subtraction ) );
			operatorDictionary.Add( "(", typeof( ParenthesisOpen ) );
			operatorDictionary.Add( ")", typeof( ParenthesisClosed ) );
			operatorDictionary.Add( "!", typeof( NotOperator ) );
			operatorDictionary.Add( "!=", typeof( NotEqualTo ) );
			operatorDictionary.Add( "*=", typeof( MultiplicativeAssignment ) );
			operatorDictionary.Add( "*", typeof( Multiplication ) );
			operatorDictionary.Add( "%", typeof( ModOperator ) );
			operatorDictionary.Add( "%=", typeof( ModAssignment ) );
			operatorDictionary.Add( "||", typeof( LogicalOr ) );
			operatorDictionary.Add( "&&", typeof( LogicalAnd ) );
			operatorDictionary.Add( "<", typeof( LessThan ) );
			operatorDictionary.Add( "<=", typeof( LessOrEqual ) );
			operatorDictionary.Add( "++", typeof( Increment ) );
			operatorDictionary.Add( ">", typeof( GreaterThan ) );
			operatorDictionary.Add( ">=", typeof( GreaterOrEqual ) );
			operatorDictionary.Add( "()", typeof( InvokeOperator ) );
			operatorDictionary.Add( "==", typeof( EqualTo ) );
			operatorDictionary.Add( "+=", typeof( AdditiveAssignment ) );
			operatorDictionary.Add( "=", typeof( Assignment ) );
			operatorDictionary.Add( "--", typeof( Decrement ) );
			operatorDictionary.Add( "/", typeof( Division ) );
			operatorDictionary.Add( "/=", typeof( DivisiveAssignment ) );
			operatorDictionary.Add( "+", typeof( Addition ) );
		}
	}
	class SubtractiveAssignment : Operator
	{
		public SubtractiveAssignment ()
		{
			Precedance = 14;
		}
	}
	class Subtraction : Operator
	{
		public Subtraction ()
		{
			Precedance = 4;
		}
	}
	class ParenthesisOpen : Operator
	{
		public ParenthesisOpen ()
		{
			Precedance = 1;
		}
	}
	class ParenthesisClosed : Operator
	{
		public ParenthesisClosed ()
		{
			Precedance = 1;
		}
	}
	class NotOperator : Operator
	{
		public NotOperator ()
		{
			Precedance = 2;
		}
	}
	class NotEqualTo : Operator
	{
		public NotEqualTo ()
		{
			Precedance = 7;
		}
	}
	class MultiplicativeAssignment : Operator
	{
		public MultiplicativeAssignment ()
		{
			Precedance = 14;
		}
	}
	class Multiplication : Operator
	{
		public Multiplication ()
		{
			Precedance = 3;
		}
	}
	class ModOperator : Operator
	{
		public ModOperator ()
		{
			Precedance = 3;
		}
	}
	class ModAssignment : Operator
	{
		public ModAssignment ()
		{
			Precedance = 14;
		}
	}
	class LogicalOr : Operator
	{
		public LogicalOr ()
		{
			Precedance = 12;
		}
	}
	class LogicalAnd : Operator
	{
		public LogicalAnd ()
		{
			Precedance = 11;
		}
	}
	class LessThan : Operator
	{
		public LessThan ()
		{
			Precedance = 6;
		}
	}
	class LessOrEqual : Operator
	{
		public LessOrEqual ()
		{
			Precedance = 6;
		}
	}
	class Increment : Operator
	{
		public Increment ()
		{
			Precedance = 2;
		}
	}
	class GreaterThan : Operator
	{
		public GreaterThan ()
		{
			Precedance = 6;
		}
	}
	class GreaterOrEqual : Operator
	{
		public GreaterOrEqual ()
		{
			Precedance = 6;
		}
	}
	class InvokeOperator : Operator
	{
		public InvokeOperator ()
		{
			Precedance = 1;
		}
	}
	class EqualTo : Operator
	{
		public EqualTo ()
		{
			Precedance = 7;
		}
	}
	class AdditiveAssignment : Operator
	{
		public AdditiveAssignment ()
		{
			Precedance = 14;
		}
	}
	class Assignment : Operator
	{
		public Assignment ()
		{
			Precedance = 14;
		}
	}
	class Decrement : Operator
	{
		public Decrement ()
		{
			Precedance = 2;
		}
	}
	class Division : Operator
	{
		public Division ()
		{
			Precedance = 3;
		}
	}
	class DivisiveAssignment : Operator
	{
		public DivisiveAssignment ()
		{
			Precedance = 14;
		}
	}
	class Addition : Operator
	{
		public Addition ()
		{
			Precedance = 4;
		}
	}
	class Unidentified : Operator
	{
		public Unidentified ()
		{
			Precedance = 0;
		}
	}
}
