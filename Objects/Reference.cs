namespace Kento
{
	public class Reference : Value
	{
		protected int index;
		public Value ReferencingValue
		{
			get { return Compiler.GetValue( index ); }
			set { Compiler.SetValue( index, value ); }
		}

		public Reference ( Value ValueToReference )
		{
			index = Compiler.StoreValue( ValueToReference );
			Compiler.RegisterReference( this, index );
		}
		public Reference ( int Index )
		{
			index = Index;
			if ( index != -1 ) //NullReference case
				Compiler.RegisterReference( this, index );
		}
		public void ChangeReference ( Reference Reference )
		{
			index = Reference.index;
		}
		public void ChangeReference ( int Index )
		{
			Dereference();
			index = Index;
			Compiler.RegisterReference( this, index );
		}
		public override Value Clone ()
		{
			return new Reference( ReferencingValue.Clone() );
		}
		public HardReference GetHardReference ()
		{
			return new HardReference( index );
		}
		public virtual void Dereference ()
		{
			Compiler.Deference( this, index );
			index = -1;
		}
	}
	class NullReference : Reference
	{
		private static NullReference value = new NullReference();
		public static NullReference Value
		{
			get { return value; }
			set { NullReference.value = value; }
		}

		public NullReference ()
			: base( -1 ) { }
	}
}
