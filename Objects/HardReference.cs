namespace Kento
{
	public class HardReference : Reference
	{
		public HardReference ( int Index )
			: base( Index ) { }

		public override Value Clone ()
		{
			return this;
		}

		public void ForceFree ()
		{
			Accessable = false;
			if ( Index == -1 ) return;
			Compiler.Dereference( this, Index );
			Index = -1;
		}

		public Reference ToNormalReference ()
		{
			var toReturn = new Reference( Index );
			ForceFree();
			return toReturn;
		}

		public override void Dereference ()
		{
			//Hard reference can't get dereferenced automatically
		}

		public override string ToString ()
		{
			return "HardRefTo: " + ReferencingValue;
		}
	}
}