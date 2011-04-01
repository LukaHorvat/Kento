using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Number : Literal
	{
		private double value;
		public double Val
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public override string ToString ()
		{
			return value.ToString();
		}
		public override Value Evaluate ()
		{
			return this;
		}
		public Number ( double Value )
		{
			value = Value;
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
		public override Value Clone ()
		{
			return new Number( value );
		}
	}
}
