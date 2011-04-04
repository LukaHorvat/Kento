using System.Collections.Generic;

namespace Kento
{
	class Boolean : Literal
	{
		private bool value;
		public bool Val
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public override string ToString ()
		{
			return value + "";
		}
		public override Value Evaluate ()
		{
			return this;
		}
		public Boolean ( bool Value )
		{
			value = Value;
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
		public override Value Clone ()
		{
			return new Boolean( value );
		}
	}
}
