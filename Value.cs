using System.Collections.Generic;

namespace Kento
{
	public abstract class Value : Token
	{
		public virtual Value Evaluate ()
		{
			return this;
		}
		public virtual Array ToArray ()
		{
			return new Array( new Reference( this ) );
		}
		public virtual List<Token> Tokenize ()
		{
			return new List<Token>( new [] { (Token)this } );
		}
		public virtual Value Clone ()
		{
			return this;
		}
	}
	class NoValue : Value
	{
		static NoValue noValue = new NoValue();
		public static NoValue Value
		{
			get { return noValue; }
			set { noValue = value; }
		}
		public override Value Evaluate ()
		{
			return Value;
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
		public override Value Clone ()
		{
			return Value;
		}
	}
}
