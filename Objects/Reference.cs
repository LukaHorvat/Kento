using System;

namespace Kento
{
	public delegate void ChangeHandler ( Value NewValue );
	public delegate Value ExternalGetter ();

	public class Reference : Value
	{
		static long nextID;
		public static long NextID { get { return nextID++; } }

		int index = -1;
		public int Index
		{
			get { return index; }
			set
			{
				if ( index != -1 ) Dereference();
				index = value;
				if ( index != -1 )
				{
					Compiler.RegisterReference( this, index );
					Accessable = true;
				}
			}
		}
		public bool Accessable { get; set; }
		public ChangeHandler OnChange { get; set; }
		public ExternalGetter External { get; set; }
		public string Identifier { get; set; }
		public long ID { get; private set; }

		public Value ReferencingValue
		{
			get
			{
				if ( External != null )
				{
					var toReturn = External();

					return toReturn;
				}
				return Compiler.GetValue( Index );

			}
			set
			{
				if ( OnChange != null )
				{
					if ( ReferencingValue.GetType() == value.GetType() )
					{
						Compiler.SetValue( Index, value );
						OnChange( value );
					} else throw new Exception( "Can't set a value with an attached OnChange event to a value of different type" );
				} else
				{
					ChangeReference( value );
				}
			}
		}

		public Reference ( ExternalMember Member )
		{
			Static = Member.Static;
			if ( Member is ExternalProperty )
			{
				var prop = Member as ExternalProperty;
				if ( prop.OnChange != null ) OnChange = prop.OnChange;
				if ( prop.External != null ) External = prop.External;
			}
			Value toStore = Member;
			ID = NextID;
			Index = Compiler.StoreValue( toStore );
			Compiler.RegisterReference( this, Index );
		}

		public Reference ( Value ValueToReference, bool ForceNew = false )
			: this( Compiler.StoreValue( ValueToReference, ForceNew ) ) { }

		public Reference ( Reference Reference )
			: this( Reference.Index ) { }

		public Reference ( int Index )
		{
			Static = Compiler.GetValue( Index ).Static;
			this.Index = Index;
			ID = NextID;
		}

		public void ChangeReference ( Value Value )
		{
			ChangeReference( Compiler.StoreValue( Value ) );
		}

		public void ChangeReference ( Reference Reference )
		{
			ChangeReference( Reference.Index );
		}

		public void ChangeReference ( int Index )
		{
			this.Index = Index;
		}

		public override Value Clone ()
		{
			if ( ReferencingValue is Literal ) return CloneWithValue();
			return new Reference( Index ) { Accessable = true };
		}

		public Reference CloneWithValue ()
		{
			return new Reference( ReferencingValue.Clone() );
		}

		public HardReference GetHardReference ()
		{
			return new HardReference( Index );
		}

		public virtual void Dereference ()
		{
			Accessable = false;
			Compiler.Dereference( this, Index );
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
			if ( ReferencingValue == null ) return "Null Reference";
			return "Reference(" + ID + ")<" + ReferencingValue + ">";
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