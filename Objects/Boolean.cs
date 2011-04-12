using System.Collections.Generic;

namespace Kento
{
	internal class Boolean : Literal
	{
		private bool value;

		public Boolean(bool Value)
		{
			value = Value;
		}

		public bool Val
		{
			get { return value; }
			set { this.value = value; }
		}

		public override string ToString()
		{
			return value + "";
		}

		public override Value Evaluate()
		{
			return this;
		}

		public override List<Token> Tokenize()
		{
			return new List<Token>(new[]{(Token) this});
		}

		public override Value Clone()
		{
			return new Boolean(value);
		}
	}
}