namespace Kento
{
	public class Reference : Value
	{
		int index;
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
		public override Value Clone ()
		{
			return new Reference( ReferencingValue.Clone() );
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
