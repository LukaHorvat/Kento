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
			return this;
		}
		public override void FreeMemory ()
		{
			//Hard references don't get freed when the scope is exited
		}
		public void ForceFree ()
		{
			Compiler.FreeMemory( index );
		}
	}
}
