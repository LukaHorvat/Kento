namespace Kento
{
	public delegate void ChangeHandler ( Value NewValue );
	public delegate Value ExternalGetter ();

	public class Reference : Value
	{
		public int Index { get; set; }
		public bool Accessable { get; set; }
		public ChangeHandler OnChange { get; set; }
		public ExternalGetter External { get; set; }
		public string Identifier { get; set; }

		public Reference ( ExternalMember Member )
		{
			Static = Member.Static;
			Value toStore;
			if ( Member is ExternalProperty )
			{
				var prop = Member as ExternalProperty;
				toStore = prop.Value;
				if ( prop.OnChange != null ) OnChange = prop.OnChange;
				if ( prop.External != null ) External = prop.External;
			} else toStore = Member;
			Index = Compiler.StoreValue( toStore, Member );
			Compiler.RegisterReference( this, Index );
		}

		public Reference ( Value ValueToReference )
			: this( ValueToReference is Reference ? ( ValueToReference as Reference ).Index : Compiler.StoreValue( ValueToReference, ValueToReference ) ) { }

		public Reference ( Reference Reference )
			: this( Reference.Index ) { }

		public Reference ( int Index )
		{
			Static = Compiler.GetValue( Index ).Static;
			this.Index = Index;
			if ( this.Index != -1 ) //NullReference case
				Compiler.RegisterReference( this, this.Index );
		}

		public Value ReferencingValue
		{
			get
			{
				if ( External != null ) return External();
				Value toReturn = Compiler.GetValue( Index );
				return toReturn;
			}
			set
			{
				if ( OnChange != null )
				{
					Compiler.SetValue( Index, value );
					OnChange( value );
				} else
				{
					ChangeReference( value );
				}
			}
		}

		public void ChangeReference ( Value Value )
		{
			ChangeReference( Compiler.StoreValue( Value, Value ) );
		}

		public void ChangeReference ( Reference Reference )
		{
			ChangeReference( Reference.Index );
		}

		public void ChangeReference ( int Index )
		{
			Compiler.Dereference( this, this.Index );
			this.Index = Index;
			Compiler.RegisterReference( this, this.Index );
		}

		public override Value Clone ()
		{
			return this;
		}

		public HardReference GetHardReference ()
		{
			return new HardReference( Index );
		}

		public virtual void Dereference ()
		{
			Accessable = false;
			if ( Index == -1 ) return;
			Compiler.Dereference( this, Index );
			Index = -1;
		}

		public override bool Equals ( object Obj )
		{
			var reference = Obj as Reference;
			if ( reference != null )
			{
				int myIndex = Index;
				int referenceIndex = reference.Index;
				return myIndex == referenceIndex;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return Index;
		}

		public override string ToString ()
		{
			return "RefTo: " + ReferencingValue;
		}
	}

	internal class NullReference : Reference
	{
		private static NullReference value = new NullReference();

		public NullReference ()
			: base( -1 ) { }

		public static NullReference Value
		{
			get { return value; }
			set { NullReference.value = value; }
		}
	}
}