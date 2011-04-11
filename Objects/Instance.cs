using System;

namespace Kento
{
	[Flags]
	public enum InstanceFlags
	{
		NoFlags = 0x00,
		Indexable = 0x01
	}

	public class Instance : Value, IFunction, IHasMembers
	{
		public Scope Identifiers { get; set; }
		public InstanceFlags Flags { get; set; }

		public Instance ( IClass Type )
			: this( Type.Identifiers, Type.Flags ) { }

		public Instance ( InstanceFlags Flags )
			: this( new Scope(), Flags ) { }

		public Instance ( Scope Identifiers, InstanceFlags Flags )
		{
			this.Flags = Flags;
			this.Identifiers = Identifiers.Clone();
			RewireFunctions();
		}
		void RewireFunctions ()
		{
			foreach ( var pair in Identifiers )
			{
				if ( pair.Value.ReferencingValue is Function )
				{
					( pair.Value.ReferencingValue as Function ).Scope = Identifiers;
				}
			}
		}
		public override Value Clone ()
		{
			return new Instance( Identifiers, Flags );
		}
		public virtual Value Invoke ( Array Arguments )
		{
			if ( Identifiers.ContainsKey( "Constructor" ) )
			{
				var constructor = Identifiers[ "Constructor" ].ReferencingValue;
				if ( constructor is IFunction ) ( constructor as IFunction ).Invoke( Arguments );
			}
			return this;
		}
	}
}
