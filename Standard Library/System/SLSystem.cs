using System;
using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	internal class SLSystem : ILibrarySegment
	{
		#region ILibrarySegment Members

		public ExternalClass Load ()
		{
			return new ExternalClass( "System", InstanceFlags.NoFlags,
				 new ExternalProperty( "Time", true, GetTime ),
				 new ExternalProperty( "MemoryUsage", true, GetMemoryUsage ),
				 new ExternalFunction( "LocationOf", true, LocationOf ) );
		}

		#endregion

		public Value GetTime ()
		{
			return new Number( Compiler.RunningTime );
		}
		public Value GetMemoryUsage ()
		{
			return new Number( Compiler.GetMemoryUsage() );
		}
		public Value LocationOf ( Array Arguments )
		{
			var adresses = Arguments.Arr.Select( Reference => new Number( Reference.Index ) ).Cast<Value>();
			return new Array( adresses.Select( X => new Reference( X ) ).ToList() );
		}
	}
}