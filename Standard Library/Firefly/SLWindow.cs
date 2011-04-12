using System;
using FireflyGL;

namespace Kento
{
	public class SLWindow : Instance
	{
		Function onLoad;
		public SLWindow ()
			: base( InstanceFlags.NoFlags )
		{

		}
		public override Value Invoke ( Array Arguments )
		{
			if ( Arguments.Arr[ 0 ].ReferencingValue is Function ) onLoad = Arguments.Arr[ 0 ].ReferencingValue as Function;
			Firefly.Initialize( 800, 500, "Firefly Window", LoadEventInterface, true );
			return this;
		}
		public void LoadEventInterface ( object Target, EventArgs Args )
		{
			if ( onLoad != null ) onLoad.Invoke();
		}
	}
}