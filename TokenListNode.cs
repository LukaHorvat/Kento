namespace Kento
{
	internal class TokenListNode
	{
		public TokenListNode(Token Token)
		{
			Value = Token;
		}

		public TokenListNode() {}
		public TokenListNode Next { get; set; }

		public TokenListNode Previous { get; set; }

		public Token Value { get; set; }
	}
}