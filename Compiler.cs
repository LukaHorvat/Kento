using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Kento.Utility;

namespace Kento
{
	internal enum FallThroughType
	{
		Break = 0,
		Continue,
		Return,
		NoFallthrough
	}

	struct DebugOptions
	{
		public bool OutputProfilerInfo { get; set; }
		public bool OutputMemoryAllocation { get; set; }
		public bool OutputDestruction { get; set; }
	}

	internal class Compiler
	{
		private static Stopwatch timer;

		private static readonly LinkedList<Scope> scopeList = new LinkedList<Scope>();

		private static readonly List<Value> memory = new List<Value>();
		private static readonly Stack<int> availabilityStack = new Stack<int>();
		private static readonly List<LinkedList<Reference>> referenceList = new List<LinkedList<Reference>>();

		public static DebugOptions Options { get; set; }

		static Compiler ()
		{
			Fallthrough = FallThroughType.NoFallthrough;
		}

		public static float RunningTime
		{
			get { return timer.ElapsedTicks / (float)Stopwatch.Frequency * 1000; }
		}

		public static bool Runtime { get; set; }
		public static Scope GlobalScope { get; set; }

		public static FallThroughType Fallthrough { get; set; }

		public static bool PendingDot { get; set; }
		public static bool PendingDotIsStatic { get; set; }

		public static bool StoreValuesOutOfScope { get; set; }

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
			for ( LinkedListNode<Scope> node = scopeList.Last ; node != null ; node = node.Previous )
			{
				if ( node.Value.ContainsKey( Name ) ) return node.Value[ Name ];
				if ( node.Value.Blocking ) break;
			}
			if ( GlobalScope.ContainsKey( Name ) ) return GlobalScope[ Name ];
			return NullReference.Value;
		}

		public static void SetAlias ( string Name, Reference Reference )
		{
			Reference.Accessable = true;
			scopeList.Last.Value[ Name ] = Reference;
		}

		public static void SetAlias ( string Name, int Index )
		{
			SetAlias( Name, new Reference( Index ) );
		}

		public static void SetAsCurrentScope ( Scope Scope )
		{
			Scope.Blocking = true;
			scopeList.AddLast( Scope );
		}

		public static void EnterScope ()
		{
			scopeList.AddLast( new Scope() );
		}

		public static void ExitScope ( bool DoNotDestroy = false )
		{
			Scope scope = scopeList.Last.Value;
			if ( !DoNotDestroy )
			{
				foreach ( var pair in scope )
				{
					pair.Value.Accessable = false;
				}
			}
			foreach ( var reference in scope.NewReferences )
			{
				if ( !reference.Accessable )
				{
					if ( Options.OutputDestruction ) Console.WriteLine( "[DEBUG] Destroying reference identifiable by: " + reference.Identifier );
					reference.Dereference();
				}
			}
			scope.Blocking = false;
			scopeList.RemoveLast();
		}

		public static Scope GetCurrentScope ()
		{
			return scopeList.Last.Value;
		}

		public static int StoreValue ( Value Value, bool ForceNew = false )
		{
			if ( Value is Reference ) return ( Value as Reference ).Index;
			if ( Value is Literal ) ForceNew = true;

			int foundIndex = memory.IndexOf( Value );
			if ( !ForceNew && foundIndex != -1 ) return foundIndex;

			int index;
			if ( availabilityStack.Count > 0 )
			{
				index = availabilityStack.Pop();
				memory[ index ] = Value;
			} else
			{
				index = memory.Count;
				memory.Add( Value );
				referenceList.Add( new LinkedList<Reference>() );
			}
			if ( Options.OutputMemoryAllocation ) Console.WriteLine( "[DEBUG] New memory reservation at {0}", index );
			return index;
		}

		public static void Destroy ( int Index )
		{
			memory[ Index ].Destroy();
			FreeMemoy( Index );
		}

		public static void FreeMemoy ( int Index )
		{
			memory[ Index ] = null;
			availabilityStack.Push( Index );
		}

		public static void RegisterReference ( Reference Reference, int Index )
		{
			if ( memory[ Index ] == null ) return;
			referenceList[ Index ].AddLast( Reference );
			if ( scopeList.Count > 0 )
			{
				if ( StoreValuesOutOfScope && scopeList.Last.Previous != null )
					scopeList.Last.Previous.Value.NewReferences.AddLast( Reference );
				else
					scopeList.Last.Value.NewReferences.AddLast( Reference );
			}
		}

		public static void Dereference ( Reference Reference, int Index )
		{
			referenceList[ Index ].Remove( Reference );

			if ( memory[ Index ] == null ) return;
			if ( referenceList[ Index ].Count == 0 ) Destroy( Index );
		}

		public static void Run ( List<Token> Code )
		{
			StandardLibrary.Load();
			Scope defaultScope = StandardLibrary.Library;
			GlobalScope = defaultScope;
			scopeList.AddLast( defaultScope );

			Runtime = true;
			timer = new Stopwatch();
			timer.Start();

			new CodeBlock( Code ).Run();

			timer.Stop();
		}

		public static void RunFromFile ( string Path, DebugOptions Options )
		{
			string source;
			using ( var reader = new StreamReader( Path ) )
			{
				source = reader.ReadToEnd();
			}
			RunFromSource( source, Options );
		}

		public static void RunFromSource ( string Code, DebugOptions Options )
		{
			Compiler.Options = Options;
			Run( Tokenizer.ParseInfixString( Code ).Tokenize() );
			if ( Options.OutputProfilerInfo ) Profiler.OutputTime();
		}

		internal static Reference Reserve ( object Sender )
		{
			return new Reference( StoreValue( NoValue.Value, true ) );
		}

		public static int GetMemoryUsage ()
		{
			return memory.Count - availabilityStack.Count;
		}
	}
}