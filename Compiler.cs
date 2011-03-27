using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Kento
{
	class Compiler
	{
		static float lastRunningTime;
		public static float LastRunningTime
		{
			get { return Compiler.lastRunningTime; }
			set { Compiler.lastRunningTime = value; }
		}

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
			defaultScope.Identifiers = Compiler.LoadStandardLibrary();
			scopeStack.Push( defaultScope );

			runtime = true;
			Stopwatch timer = new Stopwatch(); timer.Start();

			if ( Code[ 0 ] is CodeBlock )
			{
				( Code[ 0 ] as CodeBlock ).Run();
			} else
			{
				new CodeBlock( Code ).Run();
			}

			timer.Stop(); lastRunningTime = timer.ElapsedTicks / (float)Stopwatch.Frequency * 1000;
		}
		public static Dictionary<string, Value> LoadStandardLibrary ()
		{
			var lib = new Dictionary<string, Value>();
			var asm = Assembly.GetExecutingAssembly();
			var externalFunctions = asm.GetTypes().Where( x => x.IsSubclassOf( typeof( ExternalFunction ) ) );

			foreach ( System.Type fn in externalFunctions )
			{
				ExternalFunction instance = ( Activator.CreateInstance( fn ) as ExternalFunction );
				lib[ instance.Representation ] = instance;
			}

			return lib;
		}
	}
}
