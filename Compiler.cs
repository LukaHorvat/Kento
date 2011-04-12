using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

	[Flags]
	internal enum RuntimeFlags
	{
		Default = 0x00,
		Debug = 0x01
	}

	internal class Compiler
	{
		private static Stopwatch timer;

		private static readonly LinkedList<Scope> scopeList = new LinkedList<Scope>();

		private static readonly List<Value> memory = new List<Value>();
		private static readonly Stack<int> availabilityStack = new Stack<int>();
		private static readonly List<LinkedList<Reference>> referenceList = new List<LinkedList<Reference>>();

		private static FallThroughType fallthrough = FallThroughType.NoFallthrough;

		public static float RunningTime
		{
			get { return timer.ElapsedTicks/(float) Stopwatch.Frequency*1000; }
		}

		public static bool Runtime { get; set; }
		public static Scope GlobalScope { get; set; }

		public static FallThroughType Fallthrough
		{
			get { return fallthrough; }
			set { fallthrough = value; }
		}

		public static bool PendingDot { get; set; }
		public static bool PendingDotIsStatic { get; set; }

		public static Value GetValue(int Index)
		{
			Value result = NoValue.Value;
			if (Index < memory.Count && Index >= 0)
			{
				result = memory[Index];
			}
			return result;
		}

		public static void SetValue(int Index, Value Value)
		{
			if (Index < memory.Count && Index > 0) memory[Index] = Value;
		}

		public static Reference Identify(string Name)
		{
			for (LinkedListNode<Scope> node = scopeList.Last; node != null; node = node.Previous)
			{
				if (node.Value.ContainsKey(Name)) return node.Value[Name];
			}
			return NullReference.Value;
		}

		public static void SetAlias(string Name, Reference Reference)
		{
			bool set = false;
			for (LinkedListNode<Scope> node = scopeList.Last; node != null; node = node.Previous)
			{
				if (node.Value.ContainsKey(Name))
				{
					node.Value[Name] = Reference;
					set = true;
				}
			}
			if (!set) scopeList.Last.Value[Name] = Reference;
		}

		public static void SetAliasInCurrentScope(string Name, Reference Reference)
		{
			scopeList.Last.Value[Name] = Reference;
		}

		public static void SetAlias(string Name, int Index)
		{
			SetAlias(Name, new Reference(Index));
		}

		public static void SetAliasInCurrentScope(string Name, int Index)
		{
			SetAliasInCurrentScope(Name, new Reference(Index));
		}

		public static void SetAsCurrentScope(Scope Scope)
		{
			scopeList.AddLast(Scope);
		}

		public static void EnterScope()
		{
			scopeList.AddLast(new Scope());
		}

		public static void ExitScope(bool DoNotDestroy = false)
		{
			if (!DoNotDestroy)
			{
				Scope scope = scopeList.Last.Value;
				foreach (var pair in scope)
				{
					pair.Value.Dereference();
				}
			}
			scopeList.RemoveLast();
		}

		public static Scope GetCurrentScope()
		{
			return scopeList.Last.Value;
		}

		public static int StoreValue(Value Value)
		{
			int foundIndex = memory.IndexOf(Value);
			if (foundIndex != -1) return foundIndex;

			int index;
			if (availabilityStack.Count > 0)
			{
				index = availabilityStack.Pop();
				memory[index] = Value;
			}
			else
			{
				index = memory.Count;
				memory.Add(Value);
				referenceList.Add(new LinkedList<Reference>());
			}
			return index;
		}

		public static void FreeMemory(int Index)
		{
			memory[Index] = null;
			availabilityStack.Push(Index);
		}

		public static void RegisterReference(Reference Reference, int Index)
		{
			referenceList[Index].AddLast(Reference);
		}

		public static void Deference(Reference Reference, int Index)
		{
			referenceList[Index].Remove(Reference);
			if (referenceList[Index].Count == 0)
			{
				FreeMemory(Index);
			}
		}

		public static void Run(List<Token> Code)
		{
			StandardLibrary.Load();
			Scope defaultScope = StandardLibrary.Library;
			GlobalScope = defaultScope;
			scopeList.AddLast(defaultScope);

			Runtime = true;
			timer = new Stopwatch();
			timer.Start();

			new CodeBlock(Code).Run();

			timer.Stop();
		}

		public static void RunFromFile(string Path, RuntimeFlags Options)
		{
			string source;
			using (var reader = new StreamReader(Path))
			{
				source = reader.ReadToEnd();
			}
			RunFromSource(source, Options);
		}

		public static void RunFromSource(string Code, RuntimeFlags Options)
		{
			Run(Tokenizer.ParseInfixString(Code).Tokenize());
			if ((Options & RuntimeFlags.Debug) == RuntimeFlags.Debug) Profiler.OutputTime();
		}

		internal static Reference Reserve()
		{
			int index;
			if (availabilityStack.Count > 0)
			{
				index = availabilityStack.Pop();
				memory[index] = NoValue.Value;
			}
			else
			{
				index = memory.Count;
				memory.Add(NoValue.Value);
				referenceList.Add(new LinkedList<Reference>());
			}
			return new Reference(index);
		}
	}
}