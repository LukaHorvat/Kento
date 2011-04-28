using System;
using FireflyGL;

namespace Kento
{
	public class SLWindow : Instance, IUpdatable
	{
		Function onLoad, onUpdate;
		public SLWindow ()
			: base( InstanceFlags.NoFlags )
		{

		}
		public override Value Invoke ( List Arguments )
		{
			var list = Arguments.GetValues();
			if ( list[ 0 ] is Function ) onLoad = list[ 0 ] as Function;
			if ( list[ 1 ] is Function ) onUpdate = list[ 1 ] as Function;
			Firefly.Initialize( 800, 500, "Firefly Window", LoadEventInterface, true );
			return this;
		}
		public void LoadEventInterface ( object Target, EventArgs Args )
		{
			if ( onUpdate != null ) Firefly.AddToUpdateList( this );
			if ( onLoad != null ) onLoad.Invoke();
		}
		public void Update ()
		{
			onUpdate.Invoke();
		}
	}
}