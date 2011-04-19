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
		public override Value Invoke ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Function ) onLoad = Arguments.Arr[ 0 ].ReferencingValue as Function;
			if ( Arguments.GetReferenceAtIndex( 1 ).ReferencingValue is Function ) onUpdate = Arguments.Arr[ 1 ].ReferencingValue as Function;
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