using System.Collections.Generic;

namespace Kento
{
	internal class String : Literal
	{
		private string value;

		public String(string Value)
		{
			value = Value;
		}

		public string Val
		{
			get { return value; }
			set { this.value = value; }
		}

		public override string ToString()
		{
			return value;
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
			return new String(value);
		}
	}
}