using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FireflyGL;

namespace Kento
{
	class SLFirefly : ILibrarySegment
	{
		Reference color;
		public ExternalClass Load ()
		{
			return new ExternalClass( "Firefly", InstanceFlags.NoFlags
				, new ExternalFunction( "Initialize", true, Initialize )
				, new ExternalProperty( "BackgroundColor", true, BackgroundColor )
				, new ExternalFunction( "SetBackgroundColor", true, SetBackgroundColor ) );
		}
		public Value Initialize ( Array Arguments )
		{
			color = new Reference( new SLColor() );
			return new SLWindow().Invoke( Arguments );
		}
		public Value BackgroundColor ()
		{
			return color;
		}
		public Value SetBackgroundColor ( Array Arguments )
		{
			var aColor = Arguments.GetReferenceAtIndex( 0 ).ReferencingValue;
			if ( aColor is SLColor )
			{
				color.ReferencingValue = aColor;
				Firefly.Window.ClearColor = ( aColor as SLColor ).GetColor();
				return NoValue.Value;
			}
			throw new Exception( "SetBackgroundColor needs a color" );
		}
	}
}
