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
		static string[] multisymboledOperators = new string[] { "+=", "-=", "*=", "/=", "%=", "++", "--", ">=", "<=", "==", "!=", "()" };
		public static Expression GetRPN ( Expression Infix )
		{

			return new Expression();
		}
		public static Expression GetRPN ( string Infix )
		{
			return GetRPN( ParseInfixString( Infix ) );
		}
		public static Expression ParseInfixString ( string Infix )
		{
			var tokens = new LinkedList<Token>(); ;
			var array = splitString( Infix );
			foreach ( string str in array )
			{
				switch ( getCharType( str[ 0 ] ) )
				{
					case ParseState.Letters:
						if ( str[ 0 ] == '"' )
						{
							tokens.AddLast( new String( str.Substring( 1, str.Length - 2 ) ) );
						} else if ( str == "true" )
						{
							tokens.AddLast( new Boolean( true ) );
						} else if ( str == "false" )
						{
							tokens.AddLast( new Boolean( false ) );
						} else
						{
							tokens.AddLast( new Identifier( str ) );
						}
						break;
					case ParseState.Numbers:
						tokens.AddLast( new Number( double.Parse( str, System.Globalization.CultureInfo.InvariantCulture ) ) );
						break;
					case ParseState.Signs:
						if ( tokens.Last.Value is Identifier && str == "(" ) tokens.AddLast( new InvokeOperator() );
						if ( Operators.OperatorDictionary.ContainsKey( str ) )
						{
							tokens.AddLast( (Operator)Activator.CreateInstance( Operators.OperatorDictionary[ str ] ) );
						} else
						{
							tokens.AddLast( new Unidentified() );
						}
						break;
				}
			}
			return new Expression();
		}
		static LinkedList<string> splitString ( string Expression )
		{
			bool inQuote = false;
			for ( int i = 0 ; i < Expression.Length ; ++i )
			{
				if ( Expression[ i ] == ' ' && !inQuote )
				{
					Expression = Expression.Remove( i, 1 );
					--i;
				} else if ( Expression[ i ] == '"' ) inQuote = inQuote ? false : true;
			}//Clear spaces

			ParseState state = ParseState.Empty;
			ParseState currentState;
			var array = new LinkedList<string>();
			var buffer = new StringBuilder();
			for ( int i = 0 ; i < Expression.Length ; ++i )
			{
				if ( state == ParseState.Empty )//If no state, get it
				{
					if ( buffer.Length > 0 )
					{
						array.AddLast( buffer.ToString() );
					}
					state = getCharType( Expression[ i ] );
					currentState = state;
					buffer = new StringBuilder();
				} else
				{
					currentState = getCharType( Expression[ i ] );
				}
				if ( getCharType( Expression[ i ] ) == state )
				{
					if ( state == ParseState.Signs )//If symbols, check if there's a multisymboled operator comming
					{
						if ( i < Expression.Length - 1 && multisymboledOperators.Contains( Expression.Substring( i, 2 ) ) ) //If yes, add it to the array and advance the parser
						{
							buffer.Append( Expression.Substring( i, 2 ) );
							++i;
							state = ParseState.Empty;
						} else
						{
							buffer.Append( Expression[ i ] ); //Else just add the single symbol to the array
							state = ParseState.Empty;
						}
					} else
					{
						buffer.Append( Expression[ i ] ); //If not symbols, continue appending
					}
				} else //If the state changed, add the buffer to the array
				{
					--i;
					state = ParseState.Empty;
				}
			}
			if ( buffer.Length > 0 )
			{
				array.AddLast( buffer.ToString() );
			}
			return array;
		}
		/// <summary>
		/// Determines if the given char is a number, a letter or a symbol
		/// </summary>
		/// <param name="C"></param>
		/// <returns></returns>
		static ParseState getCharType ( char C )
		{
			if ( C >= 'A' && C <= 'z' || C == '"' || C == ' ' )
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
