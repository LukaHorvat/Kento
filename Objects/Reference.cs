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
		}
		public Reference ( int Index )
		{
			index = Index;
		}
		public void ChangeReference ( Reference Reference )
		{
			index = Reference.index;
		}
		public void ChangeReference ( int Index )
		{
			index = Index;
		}
		public override Value Clone ()
		{
			return new Reference( ReferencingValue.Clone() );
		}
		public HardReference GetHardReference ()
		{
			return new HardReference( index );
		}
		public virtual void FreeMemory ()
		{
			Compiler.FreeMemory( index );
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
