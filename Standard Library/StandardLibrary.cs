using System;
using System.Reflection;

namespace Kento
{
	class StandardLibrary
	{
		static readonly Scope library = new Scope();
		static public Scope Library { get { return library; } }

		public static void AddClass ( ExternalClass Class, string Binding )
		{
			library[ Binding ] = new Reference( Class );
		}

		public static void Load ()
		{
			var asm = Assembly.GetCallingAssembly();
			foreach ( var type in asm.GetTypes() )
			{
				if ( type.GetInterface( "ILibrarySegment" ) != null )
				{
					var segment = Activator.CreateInstance( type ) as ILibrarySegment;
					var extClass = ( segment ).Load();
					AddClass( extClass, extClass.Representation );
				}
			}
		}
	}
}
