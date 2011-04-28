using System;

namespace Kento
{
	[Flags]
	public enum InstanceFlags
	{
		NoFlags = 0x00,
		Indexable = 0x01
	}

	public class Instance : Value, IInvokable, IHasMembers
	{
		public string ClassName { get; set; }
		public string Name { get; set; }
		public Scope ParentScope { get; set; }

		public Instance ( IClass Type )
			: this( Type.Identifiers, Type.Flags ) { ClassName = Type.Name; }

		public Instance ( InstanceFlags Flags )
			: this( new Scope(), Flags ) { }

		public Instance ( Scope Identifiers, InstanceFlags Flags )
		{
			this.Flags = Flags;
			this.Identifiers = Identifiers.Clone();
			this.Identifiers.RewireFunctions();
		}

		#region IInvokable Members

		public virtual Value Invoke ( List Arguments )
		{
			if ( Identifiers.ContainsKey( "Constructor" ) )
			{
				Value constructor = Identifiers[ "Constructor" ].ReferencingValue;
				if ( constructor is IInvokable )
				{
					( constructor as IInvokable ).Invoke( Arguments );
				}
			}
			return this;
		}

		#endregion

		#region IHasMembers Members

		public Scope Identifiers { get; set; }
		public InstanceFlags Flags { get; set; }

		#endregion

		public override Value Clone ()
		{
			return new Instance( Identifiers, Flags );
		}

		public override void Destroy ()
		{
			foreach ( var pair in Identifiers )
			{
				pair.Value.Dereference();
			}
		}

		public override string ToString ()
		{
			return "Instance(" + Name + ") of <" + ClassName + ">";
		}

	}
}