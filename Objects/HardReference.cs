namespace Kento
{
	public class HardReference : Reference
	{
		public HardReference ( int Index )
			: base( Index )
		{

		}
		public override Value Clone ()
		{
			return new Reference( index );
		}
		public void ForceFree ()
		{
			Compiler.FreeMemory( index );
		}
	}
}
