using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class String : Literal
	{
		private string value;
		public string Val
		{
			get { return this.value; }
			set { this.value = value; }
		}
		public override string ToString ()
		{
			return value;
		}
		public override Value Evaluate ()
		{
			return this;
		}
		public String ( string Value )
		{
			value = Value;
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
		public override Value Clone ()
		{
			return new String( value );
		}
	}
}
