using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Kento
{
	enum FallThroughType
	{
		Break = 0,
		Continue,
		Return,
		NoFallthrough
	}

	class Compiler
	{
		static Stopwatch timer;

		static float runningTime;
		public static float RunningTime
		{
			get { return timer.ElapsedTicks / (float)Stopwatch.Frequency * 1000; }
		}

		static bool runtime;
		public static bool Runtime
		{
			get { return Compiler.runtime; }
			set { Compiler.runtime = value; }
		}

		static LinkedList<Dictionary<string, Value>> scopeList = new LinkedList<Dictionary<string, Value>>();

		static FallThroughType fallthrough = FallThroughType.NoFallthrough;
		public static FallThroughType Fallthrough
		{
			get { return Compiler.fallthrough; }
			set { Compiler.fallthrough = value; }
		}

		static Stack<int> instanceScopes = new Stack<int>();

		public static Value GetValue ( string Name )
		{
			Value result = NoValue.Value;
			for ( var node = scopeList.Last ; node != null ; node = node.Previous )
			{
				if ( node.Value.ContainsKey( Name ) )
				{
					result = node.Value[ Name ];
					break;
				}
			}
			if ( result is Identifier ) result = GetValue( result as Identifier );
			return result;
		}
		public static Value GetValue ( Identifier Identifier )
		{
			return GetValue( Identifier.Name );
		}
		public static void SetValue ( string Name, Value Value )
		{
			for ( var node = scopeList.Last ; node != null ; node = node.Previous )
			{
				if ( node.Value.ContainsKey( Name ) )
				{
					node.Value[ Name ] = Value;
					return;
				}
			}
			scopeList.Last.Value[ Name ] = Value;
		}
		public static void SetValue ( Identifier Identifier, Value Value )
		{
			if ( Identifier is ArrayIdentifier ) ( Identifier as ArrayIdentifier ).SetValue( Value );
			else SetValue( Identifier.Name, Value );
		}
		public static void MakeValueInCurrentScope ( string Name, Value Value )
		{
			scopeList.Last.Value[ Name ] = Value;
		}
		public static void MakeValueInCurrentScope ( Identifier Identifier, Value Value )
		{
			MakeValueInCurrentScope( Identifier.Name, Value );
		}
		public static void SetAsCurrentScope ( Dictionary<string, Value> Scope )
		{
			scopeList.AddLast( Scope );
		}
		public static void EnterScope ()
		{
			scopeList.AddLast( new Dictionary<string, Value>() );
		}
		public static void ExitScope ()
		{
			scopeList.RemoveLast();
		}
		public static void EnterInstanceScope ( Instance Instance )
		{
			scopeList.AddLast( Instance.Identifiers );
			instanceScopes.Push( scopeList.Count );
		}
		public static void ExitInstanceScope ()
		{
			if ( instanceScopes.Count > 0 )
			{
				if ( instanceScopes.Peek() == scopeList.Count )
				{
					scopeList.RemoveLast();
					instanceScopes.Pop();
				}
			}
		}
		public static void Run ( string Code )
		{
			Run( Tokenizer.ParseInfixString( Code ).Tokenize() );
		}
		public static void Run ( List<Token> Code )
		{
			var defaultScope = Compiler.LoadStandardLibrary();
			scopeList.AddLast( defaultScope );

			runtime = true;
			timer = new Stopwatch(); timer.Start();

			new CodeBlock( Code ).Run();

			timer.Stop();
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
