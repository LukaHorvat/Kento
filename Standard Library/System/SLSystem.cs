using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class SLSystem : ILibrarySegment
	{
		public ExternalClass Load ()
		{
			var getTime = new ExternalFunction( "GetTime", true, GetTime );

			return new ExternalClass( "System", InstanceFlags.NoFlags, getTime );
		}

		public Value GetTime ( Array Arguments )
		{
			if ( Arguments.Arr.Count > 0 ) throw new Exception( "GetTime doesn't take parameters" );
			return new Number( Compiler.RunningTime );
		}
	}
}
