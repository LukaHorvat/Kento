namespace Kento
{
	public class Token {
		public int TokenIndex { get; set; }
		public Token ()
		{
			if ( !Compiler.Runtime )
			{
				TokenIndex = Tokenizer.NumberOfTokens;
				Tokenizer.NumberOfTokens++;
			}
		}
	}
}