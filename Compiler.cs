using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Compiler
	{
		static bool runtime;
		public static bool Runtime
		{
			get { return Compiler.runtime; }
			set { Compiler.runtime = value; }
		}

		static Stack<CodeBlock> scopeStack = new Stack<CodeBlock>();
		public static CodeBlock ExecutingScope
		{
			get
			{
				if ( Compiler.scopeStack.Count > 0 ) return Compiler.scopeStack.Peek();
				else return null;
			}
			set { Compiler.scopeStack.Push( value ); }
		}
		public static void ExitScope ()
		{
			var last = scopeStack.Pop();
			foreach ( var pair in last.Identifiers )
			{
				if ( scopeStack.Peek().Identifiers.ContainsKey( pair.Key ) ) scopeStack.Peek().Identifiers[ pair.Key ] = pair.Value;
			}
		}
		public static void Run ( string Code )
		{
			Run( Tokenizer.ParseInfixString( Code ).Tokenize() );
		}
		public static void Run ( List<Token> Code )
		{
			CodeBlock defaultScope = new CodeBlock( new List<Token>() );
			defaultScope.Identifiers[ "ConsoleOutput" ] = new ConsoleOutput();
			defaultScope.Identifiers[ "ConsoleInput" ] = new ConsoleInput();
			scopeStack.Push( defaultScope );

			runtime = true;
			if ( Code[ 0 ] is CodeBlock )
			{
				( Code[ 0 ] as CodeBlock ).Run();
			} else
			{
				new CodeBlock( Code ).Run();
			}
		}
	}
}
