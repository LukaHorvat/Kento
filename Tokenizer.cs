using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kento
{
	internal enum ParseState
	{
		Numbers = 0,
		Letters,
		Signs,
		Empty
	}

	internal class Tokenizer
	{
		public static int NumberOfTokens { get; set; }
		static List<Token> originalTokenOrder = new List<Token>();

		public static string GetTokenByIndex ( int Index )
		{
			return originalTokenOrder[ Index ].ToString();
		}

		public static Value ParseInfixString ( string Infix )
		{
			var tokens = new TokenList();
			IEnumerable<string> array = SplitString( Infix );
			foreach ( string str in array )
			{
				switch ( GetCharType( str[ 0 ] ) )
				{
					case ParseState.Letters:
						if ( str[ 0 ] == '"' )
						{
							tokens.Add( new String( str.Substring( 1, str.Length - 2 ) ) );
						} else
						{
							if ( Operators.OperatorDictionary.ContainsKey( str ) )
							{
								tokens.Add( (Token)Activator.CreateInstance( Operators.OperatorDictionary[ str ] ) );
							} else
							{
								switch ( str )
								{
									case "true":
										tokens.Add( new Boolean( true ) );
										break;
									case "false":
										tokens.Add( new Boolean( false ) );
										break;
									default:
										tokens.Add( new Identifier( str ) );
										break;
								}
							}
						}
						break;
					case ParseState.Numbers:
						tokens.Add( new Number( double.Parse( str, CultureInfo.InvariantCulture ) ) );
						break;
					case ParseState.Signs:
						string str1 = str;
						Func<BracketType, bool> fn = ( X => Operators.RepresentationDictionary[ X.OpeningBracket ] == str1 );
						if ( Operators.Brackets.Any( fn ) )
						{
							if ( str1 == "[" && !( tokens.Last.Value is Identifier ) && !( tokens.Last.Value is ParenthesisClosed ) ) tokens.Add( new MakeArray() );
							else tokens.Add( (Token)Activator.CreateInstance( Operators.Brackets.First( fn ).SpecialOperator ) );
							tokens.Add( new ParenthesisOpen() );
						} else if ( Operators.Brackets.Any( X => Operators.RepresentationDictionary[ X.ClosingBracket ] == str1 ) )
						{
							tokens.Add( new ParenthesisClosed() );
						} else
						{
							if ( tokens.Last.Value is Identifier && str1 == "(" )
							{
								tokens.Add( new InvokeOperator() );
								tokens.Add( new ParenthesisOpen() );
							} else if ( !( tokens.Last.Value is Number || tokens.Last.Value is ParenthesisClosed || tokens.Last.Value is Identifier ) && str1 == "-" )
							{
								tokens.Add( new Number( 0 ) );
								tokens.Add( new Subtraction() );
							} else if ( Operators.OperatorDictionary.ContainsKey( str1 ) )
							{
								tokens.Add( (Operator)Activator.CreateInstance( Operators.OperatorDictionary[ str1 ] ) );
							} else
							{
								tokens.Add( new Unidentified() );
							}
						}
						break;
				}
			}
			originalTokenOrder = tokens.ToLinkedList().ToList();
			Value test = Expression.CreateValueFromExpression( tokens );
			return test;
		}

		private static IEnumerable<string> SplitString ( string Exp )
		{
			int commentStart;
			while ( ( commentStart = Exp.IndexOf( "//" ) ) != -1 )
			{
				int end = Exp.IndexOf( '\n', commentStart );
				if ( end != -1 )
				{
					Exp = Exp.Remove( commentStart, end - commentStart );
				} else Exp = Exp.Remove( commentStart );
			}
			var expression = new StringBuilder( Exp );
			expression.Replace( '\n', ' ' );
			expression.Replace( '\t', ' ' );
			expression.Replace( '\r', ' ' );
			expression.Replace( @"\n", '\n' + "" );
			expression.Replace( @"\r", '\r' + "" );
			expression.Replace( @"\t", '\t' + "" );
			var alreadyDone = new List<bool>();
			//Stores the already processed operators so the ones like ++ and + don't get processed twice
			bool inQuote = false;
			for ( int i = 0 ; i < expression.Length ; ++i ) //Quotation pass
			{
				if ( expression[ i ] == '"' ) inQuote = inQuote ? false : true;
				if ( inQuote && expression[ i ] == ' ' ) expression[ i ] = (char)7;
				alreadyDone.Add( inQuote );
			}
			for ( int i = 0 ; i < expression.Length ; ++i ) //Decimal number pass
			{
				if ( expression[ i ] == '.' && GetCharType( expression[ i - 1 ] ) == ParseState.Numbers &&
					GetCharType( expression[ i + 1 ] ) == ParseState.Numbers )
				{
					alreadyDone[ i ] = true;
				}
			}

			List<string> operatorList = Operators.RepresentationDictionary.Values.ToList();
			operatorList.Sort( new Comparison<string>( ( X, Y ) => Y.Length - X.Length ) );
			foreach ( var t in operatorList )
			{
				for ( int j = 0 ; j <= expression.Length - t.Length ; j++ )
				{
					if ( !alreadyDone[ j ] && expression.ToString( j, t.Length ) == t )
					{
						for ( int k = 0 ; k < t.Length ; ++k )
						{
							alreadyDone[ j + k ] = true;
						}
						alreadyDone.Insert( j + t.Length, true );
						alreadyDone.Insert( j, true );
						expression = expression.Insert( j + t.Length, " " );
						expression = expression.Insert( j, " " );
					}
				}
			}

			var temp = new LinkedList<string>( expression.ToString().Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ) );
			for ( LinkedListNode<string> node = temp.First ; node != null ; node = node.Next )
			{
				node.Value = node.Value.Replace( '\a', ' ' );
			}
			return temp;
		}

		/// <summary>
		/// Determines if the given char is a number, a letter or a symbol
		/// </summary>
		/// <param name="C"></param>
		/// <returns></returns>
		private static ParseState GetCharType ( char C )
		{
			if ( ( C >= 'A' && C <= 'Z' ) || ( C >= 'a' && C <= 'z' ) || C == '"' || C == ' ' )
			{
				return ParseState.Letters;
			}
			if ( C >= '0' && C <= '9' )
			{
				return ParseState.Numbers;
			}
			return ParseState.Signs;
		}
	}
}