using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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

		public static float RunningTime
		{
			get { return timer.ElapsedTicks / (float)Stopwatch.Frequency * 1000; }
		}

		static bool runtime;
		public static bool Runtime
		{
			get { return runtime; }
			set { runtime = value; }
		}

		static readonly LinkedList<Dictionary<string, Reference>> scopeList = new LinkedList<Dictionary<string, Reference>>();

		static readonly List<Value> memory = new List<Value>();
		static readonly Stack<int> availabilityStack = new Stack<int>();

		static FallThroughType fallthrough = FallThroughType.NoFallthrough;
		public static FallThroughType Fallthrough
		{
			get { return fallthrough; }
			set { fallthrough = value; }
		}

		public static Value GetValue ( int Index )
		{
			Value result = NoValue.Value;
			if ( Index < memory.Count && Index >= 0 )
			{
				result = memory[ Index ];
			}
			return result;
		}
		public static void SetValue ( int Index, Value Value )
		{
			if ( Index < memory.Count && Index > 0 ) memory[ Index ] = Value;
		}
		public static Reference Identify ( string Name )
		{
			for ( var node = scopeList.Last ; node != null ; node = node.Previous )
			{
				if ( node.Value.ContainsKey( Name ) ) return node.Value[ Name ];
			}
			return NullReference.Value;
		}
		public static void SetAlias ( string Name, Reference Reference )
		{
			bool set = false;
			for ( var node = scopeList.Last ; node != null ; node = node.Previous )
			{
				if ( node.Value.ContainsKey( Name ) )
				{
					node.Value[ Name ] = Reference;
					set = true;
				}
			}
			if ( !set ) scopeList.Last.Value[ Name ] = Reference;
		}
		public static void SetAliasInCurrentScope ( string Name, Reference Reference )
		{
			scopeList.Last.Value[ Name ] = Reference;

		}
		public static void SetAlias ( string Name, int Index )
		{
			SetAlias( Name, new Reference( Index ) );
		}
		public static void SetAliasInCurrentScope ( string Name, int Index )
		{
			SetAliasInCurrentScope( Name, new Reference( Index ) );
		}
		public static void SetAsCurrentScope ( Dictionary<string, Reference> Scope )
		{
			scopeList.AddLast( Scope );
		}
		public static void EnterScope ()
		{
			scopeList.AddLast( new Dictionary<string, Reference>() );
		}
		public static void ExitScope ( bool DoNotDestroy = false )
		{
			if ( !DoNotDestroy )
			{
				var scope = scopeList.Last.Value;
				foreach ( var pair in scope )
				{
					pair.Value.ReferencingValue = null;

				}
			}
			scopeList.RemoveLast();
		}
		public static int StoreValue ( Value Value )
		{

			var foundIndex = memory.IndexOf( Value );
			if ( foundIndex != -1 ) return foundIndex;

			int index;
			if ( availabilityStack.Count > 0 )
			{
				index = availabilityStack.Pop();
				memory[ index ] = Value;
			} else
			{
				index = memory.Count;
				memory.Add( Value );
			}
			return index;
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
		public static Dictionary<string, Reference> LoadStandardLibrary ()
		{
			var lib = new Dictionary<string, Reference>();
			var asm = Assembly.GetExecutingAssembly();
			var externalFunctions = asm.GetTypes().Where( x => x.IsSubclassOf( typeof( ExternalFunction ) ) );

			foreach ( System.Type fn in externalFunctions )
			{
				ExternalFunction instance = ( Activator.CreateInstance( fn ) as ExternalFunction );
				lib[ instance.Representation ] = new Reference( instance );
			}

			return lib;
		}
	}
}
