using System;
using System.Reflection;

namespace Kento
{
	internal class StandardLibrary
	{
		private static readonly Scope library = new Scope();

		public static Scope Library
		{
			get { return library; }
		}

		public static void AddClass(ExternalClass Class, string Binding)
		{
			library[Binding] = new Reference(Class);
		}

		public static void Load()
		{
			Assembly asm = Assembly.GetCallingAssembly();
			foreach (System.Type type in asm.GetTypes())
			{
				if (type.GetInterface("ILibrarySegment") != null)
				{
					var segment = Activator.CreateInstance(type) as ILibrarySegment;
					ExternalClass extClass = (segment).Load();
					AddClass(extClass, extClass.Name);
				}
			}
		}
	}
}