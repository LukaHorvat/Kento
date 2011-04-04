using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	enum ParseState : int
	{
		Numbers = 0,
		Letters,
		Signs,
		Empty
	}

	class Tokenizer
	{
		public static Value ParseInfixString ( string Infix )
		{
			var tokens = new TokenList(); ;
			var array = splitString( Infix );
			foreach ( string str in array )
			{
				switch ( getCharType( str[ 0 ] ) )
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
						tokens.Add( new Number( double.Parse( str, System.Globalization.CultureInfo.InvariantCulture ) ) );
						break;
					case ParseState.Signs:
						Func<BracketType, bool> fn = ( x => Operators.RepresentationDictionary[ x.OpeningBracket ] == str );
						if ( Operators.Brackets.Any( fn ) )
						{
							tokens.Add( (Token)Activator.CreateInstance( Operators.Brackets.First( fn ).SpecialOperator ) );
							tokens.Add( new ParenthesisOpen() );
						} else if ( Operators.Brackets.Any( x => Operators.RepresentationDictionary[ x.ClosingBracket ] == str ) )
						{
							tokens.Add( new ParenthesisClosed() );
						} else
						{
							if ( tokens.Last.Value is Identifier && str == "(" ) tokens.Add( new InvokeOperator() );
							if ( tokens.Last.Value is Identifier && str == "++" ) tokens.Add( new SufixIncrement() );
							else if ( !( tokens.Last.Value is Identifier ) && str == "++" ) tokens.Add( new PrefixIncrement() );
							else if ( tokens.Last.Value is Identifier && str == "--" ) tokens.Add( new SufixDecrement() );
							else if ( !( tokens.Last.Value is Identifier ) && str == "--" ) tokens.Add( new PrefixDecrement() );
							else if ( Operators.OperatorDictionary.ContainsKey( str ) )
							{
								tokens.Add( (Operator)Activator.CreateInstance( Operators.OperatorDictionary[ str ] ) );
							} else
							{
								tokens.Add( new Unidentified() );
							}
						}
						break;
				}
			}
			var test = Expression.CreateValueFromExpression( tokens );
			return test;
		}
		static LinkedList<string> splitString ( string Exp )
		{
			var Expression = new StringBuilder( Exp );
			Expression.Replace( '\n', ' ' );
			Expression.Replace( '\t', ' ' );
			Expression.Replace( '\r', ' ' );
			Expression.Replace( @"\n", '\n' + "" );
			Expression.Replace( @"\r", '\r' + "" );
			Expression.Replace( @"\t", '\t' + "" );
			var alreadyDone = new List<bool>(); //Stores the already processed operators so the ones like ++ and + don't get processed twice
			bool inQuote = false;
			for ( int i = 0 ; i < Expression.Length ; ++i )//Quotation pass
			{
				if ( Expression[ i ] == '"' ) inQuote = inQuote ? false : true;
				if ( inQuote && Expression[ i ] == ' ' ) Expression[ i ] = (char)7;
				alreadyDone.Add( inQuote );
			}
			for ( int i = 0 ; i < Expression.Length ; ++i ) //Decimal number pass
			{
				if ( Expression[ i ] == '.' && getCharType( Expression[ i - 1 ] ) == ParseState.Numbers && getCharType( Expression[ i + 1 ] ) == ParseState.Numbers )
				{
					alreadyDone[ i ] = true;
				}
			}

			var operatorList = Operators.RepresentationDictionary.Values.ToList();
			operatorList.Sort( new Comparison<string>( compare ) );
			for ( int i = 0 ; i < operatorList.Count ; ++i )
			{
				for ( int j = 0 ; j <= Expression.Length - operatorList[ i ].Length ; j++ )
				{
					if ( !alreadyDone[ j ] && Expression.ToString( j, operatorList[ i ].Length ) == operatorList[ i ] )
					{
						for ( int k = 0 ; k < operatorList[ i ].Length ; ++k )
						{
							alreadyDone[ j + k ] = true;
						}
						alreadyDone.Insert( j + operatorList[ i ].Length, true );
						alreadyDone.Insert( j, true );
						Expression = Expression.Insert( j + operatorList[ i ].Length, " " );
						Expression = Expression.Insert( j, " " );
					}
				}
			}

			var temp = new LinkedList<string>( Expression.ToString().Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ) );
			for ( var node = temp.First ; node != null ; node = node.Next )
			{
				node.Value = node.Value.Replace( '\a', ' ' );
			}
			return temp;
		}
		static int compare ( string First, string Second )
		{
			return Second.Length - First.Length;
		}
		/// <summary>
		/// Determines if the given char is a number, a letter or a symbol
		/// </summary>
		/// <param name="C"></param>
		/// <returns></returns>
		static ParseState getCharType ( char C )
		{
			if ( ( C >= 'A' && C <= 'Z' ) || ( C >= 'a' && C <= 'z' ) || C == '"' || C == ' ' )
			{
				return ParseState.Letters;
			} else if ( C >= '0' && C <= '9' )
			{
				return ParseState.Numbers;
			} else
			{
				return ParseState.Signs;
			}
		}
	}
}
