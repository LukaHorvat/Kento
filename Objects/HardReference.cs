namespace Kento
{
	public class HardReference : Reference
	{
		public HardReference(int Index)
			: base(Index) {}

		public override Value Clone()
		{
			return new Reference(Index);
		}

		public void ForceFree()
		{
			Compiler.FreeMemory(Index);
		}

		public override Array ToArray()
		{
			return new Array(this);
		}
	}
}