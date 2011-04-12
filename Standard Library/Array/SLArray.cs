namespace Kento
{
	internal class SLArray : ExternalClass, ILibrarySegment
	{
		public SLArray(params ExternalMember[] Members)
			: base("Array", InstanceFlags.Indexable, Members) {}

		public SLArray()
			: base("Array", InstanceFlags.Indexable) {}

		#region ILibrarySegment Members

		public ExternalClass Load()
		{
			return new SLArray();
		}

		#endregion

		public override Instance MakeInstance()
		{
			return new Array();
		}
	}
}